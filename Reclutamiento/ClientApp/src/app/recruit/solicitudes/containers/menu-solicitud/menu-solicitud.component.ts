import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu-solicitud',
  templateUrl: './menu-solicitud.component.html',
  styleUrls: ['./menu-solicitud.component.scss']
})
export class MenuSolicitudComponent implements OnInit {

  loading = false;
  IdTicket: string;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  incrementoIndividual(){

  }

  incrementoMasiva(){

  }

  coberturaIndividual(){

  }

  obtenerIdSolicitud(){
    // this.loading = true;
    // this.solicitudService.solicitud()
    // .pipe(
    //   finalize(() => this.loading = false)
    // ).subscribe(
    //   data => {
    //     if (!data) {
    //       return;
    //     }
    //     localStorage.setItem('solicitud', JSON.stringify(data));
    //     this.IdTicket = '';
    //     this.router.navigate( ['/solicitud', data.idTicket] );
    //   });
  }

}
