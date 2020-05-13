import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

export function formatFileSize(bytes: any, decimalPoint: any) {
  if (bytes === 0) {
    return '0 Bytes';
  }

  let k = 1000,
      dm = decimalPoint || 2,
      sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
      i = Math.floor(Math.log(bytes) / Math.log(k));

  return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

export interface Document {
  id: number | null;
  title: string | null;
  name: string | null;
  path: string | null;
  updateDate: Date | null;
  loaded: boolean | null;
  required: boolean | null;
  file: File | null;
  error: boolean | null;
}

@Component({
  selector: 'ho1a-upload-files',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './upload-files.component.html',
  styleUrls: ['./upload-files.component.scss']
})
export class UploadFilesComponent implements OnInit {

  @Input()
  documents: Document[];

  @Input()
  readonly: boolean;

  @Input()
  uploadingFile = false;

  @Input()
  withtemplate = false;

  @Output()
  delete = new EventEmitter<Document>();

  @Output()
  uploadFile = new EventEmitter<File>();

  @Output()
  upload = new EventEmitter<Document>();

  fileToUpload: File = null;

  sizeFile = 10000000;
  errorSizeFile = false;

  constructor() { }

  ngOnInit() {
  }

  formatFileSize(bytes: any, decimalPoint: any) {
    if (bytes === 0) {
      return '0 Bytes';
    }

    let k = 1000,
      dm = decimalPoint || 2,
      sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
      i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  handleFileInput(files: FileList, item?: Document) {
    if (item) {
      if (files.length) {
        if (files.item(0).size > this.sizeFile) {
          item.error = true;
          return;
        }
      }
      item.file = files.item(0);
      item.error = false;
      this.onUploadDocument(item);
    } else {
      if (files.length) {
        if (files.item(0).size > this.sizeFile) {
          this.errorSizeFile = true;
          return;
        }
      }
      this.fileToUpload = files.item(0);
      this.errorSizeFile = false;
    }
  }

  onUploadFile() {
    this.uploadingFile = true;
    this.uploadFile.emit(this.fileToUpload);
  }

  onDeleteDocument(file: Document, index?: number) {
    this.delete.emit(file);
  }

  onUploadDocument(file: Document, index?: number) {
    this.upload.emit(file);
  }

}
