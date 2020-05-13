import { Component, ElementRef, HostListener } from '@angular/core';
import { Select } from '@ngxs/store';
import { NotificacionState } from 'src/app/store/state/notificaciones.state';
import { Observable } from 'rxjs';
import { Notificacion } from 'src/app/shared/models/notificacion';

@Component({
  selector: 'ho1a-notification-button',
  templateUrl: './notification-button.component.html',
  styleUrls: ['./notification-button.component.scss']
})
export class NotificationButtonComponent {
  @Select(NotificacionState.notificaciones) notificaciones$: Observable<Notificacion[]>;
  private show: boolean = false;

  constructor(private eRef: ElementRef) {}
  
  @HostListener('document:click', ['$event'])
  clickout(event) {
    if(!this.eRef.nativeElement.contains(event.target)) {
      this.show = false;
    } 
  }

  public onToggle(): void {
    this.show = !this.show;
  }

}
