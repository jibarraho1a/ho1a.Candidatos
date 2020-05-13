export class Ternas {

  private ternasWithCandidates: any[];

  private onlyTernas = [
    { value: 0, text: 'Sin definir' },
    { value: 1, text: 'Terna #1' },
    { value: 2, text: 'Terna #2' },
    { value: 3, text: 'Terna #3' },
    { value: 4, text: 'Terna #4' },
    { value: 5, text: 'Terna #5' },
  ];

  constructor() {
  }

  setTernasWithCandidates(ternasWithCandidates: any[]) {
    this.ternasWithCandidates = ternasWithCandidates;
  }

  getCatalogTernas() {
    return this.onlyTernas;
  }

  canChangeSpecifiedTerna(ternaIdDestino: number) {
    return true;
  }

  isTerna1Full() {
    return true;
  }

  isTerna2Full() {
    return true;
  }

  isTerna3Full() {
    return true;
  }

  isTerna4Full() {
    return true;
  }

  isTerna5Full() {
    return true;
  }

}
