import { NotificationService } from '@progress/kendo-angular-notification';
import { Injectable } from '@angular/core';
import { CoreModule } from 'src/app/core/core.module';

@Injectable({
  providedIn: CoreModule,
})
export class ToasterService {

  public animationDuration = 600;

  constructor(private notificationService: NotificationService) { }

  public show(type: 'none' | 'info' | 'error' | 'warning' | 'success', content: string): void {
    this.notificationService.show({
      content: content,
      hideAfter: 600,
      width: 350,
      cssClass: 'kendo-custom-toaster',
      position: { horizontal: 'right', vertical: 'bottom' },
      animation: { type: 'fade', duration: this.animationDuration },
      type: { style: type, icon: true }
    });
  }
}
