export interface GridOption {
  columnsDefs: ColumnsDef[];
  columnsForRow?: number;
  allowDelete?: boolean;
  allowEdith?: boolean;
  columnsDefsButton?: ColumnsDefsButton[];
}

interface ColumnsDef {
  field: string;
  title: string;
  type?: string;
  isVisible: boolean;
  isFilter: boolean;
  isExportable: boolean;
  canOrderBy: boolean;
  applyStyles?: boolean;
}

interface ColumnsDefsButton {
  key: string;
  icon: string;
  tooltip: string;
}
