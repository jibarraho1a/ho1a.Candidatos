import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FileSaver } from 'file-saver';
import * as XLSX from 'xlsx';



const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const EXCEL_EXTENSION = '.xlsx';

@Component({
  selector: 'ho1a-export',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.css']
})
export class ExportComponent implements OnInit {

  @Input()
  json: any[];

  @Input()
  columnsDefs: any[];

  constructor() { }

  ngOnInit() {
  }

  exportExcel() {
    this.exportAsExcelFile('listado');
  }

  private exportAsExcelFile(excelFileName: string): void {
    const worksheet = this.generateWorksheet();
    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, excelFileName);
  }

  private generateWorksheet(): XLSX.WorkSheet {
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.filterJson());
    if (!this.columnsDefs) {
      return;
    }
    const worksheetCopy: XLSX.WorkSheet = {};
    Object.keys(worksheet).map( (value: any, index: number) => {
      const regexNumeros = /(\d+)/g;
      const regexLetras = /^[A-Za-z]/g;
      const findRegexNumeros = value.match(regexNumeros);
      const findRegexLetras = value.match(regexLetras);
      let parteNumerica: number;
      let parteLetras: string;
      if (findRegexNumeros) {
        parteNumerica = Number.parseInt(findRegexNumeros[0]);
      }
      if (findRegexLetras) {
        parteLetras = findRegexLetras[0];
      }
      const siValueEsUnTitulo = worksheet[value].v !== undefined && parteNumerica === 1;
      const siValueEsValor = worksheet[value].v !== undefined && parteNumerica !== 1;
      const siValueEsOtraCosa = worksheet[value].v === null || worksheet[value].v === undefined;
      let column = null;
      if (siValueEsUnTitulo || siValueEsValor) {
        column = this.getColumnByName(worksheet[parteLetras + '1'].v);
      }
      if (siValueEsOtraCosa) {
        worksheetCopy[value] = worksheet[value];
      } else if (column && column.isExportable) {
        worksheetCopy[value] = {...{}, ...worksheet[value]};
        worksheetCopy[value].v = siValueEsUnTitulo ? column.title : worksheet[value].v;
      }
    });
    return worksheetCopy;
  }

  private filterJson() {
    let jsonCopy = [];
    this.json.forEach( (value: any, index: number) => {
      const row = {};
      Object.keys(value).map( (col: any, index: number) => {
        const column = this.getColumnByName(col);
        if (column && column.isExportable) {
          row[col] = value[col];
        }
      });
      jsonCopy.push(row);
    });
    return [...[], ...jsonCopy];
  }

  private getColumnByName(name: string) {
    return this.columnsDefs.find( f => f.field === name);
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: EXCEL_TYPE
    });
    FileSaver.saveAs(data, fileName + '_export_' + new Date().getTime() + EXCEL_EXTENSION);
  }

}
