export class Candidatos {

  private candidatos: any[];


  constructor() {
  }

  setCandidatos(candidato: any[]) {
    this.candidatos = candidato;
  }

  puedoAgregarCandidatoATerna(ternaId: number): boolean {
    const candidatos = this.candidatos.filter( f => f.ternaId === ternaId );
    return candidatos && candidatos.length < 3;
  }

}
