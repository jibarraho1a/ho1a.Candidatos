import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Menu } from 'src/app/shared/models/menu';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }

  getUserMock(): User {
    return {
      id: 1380,
      nombre: 'Daniel',
      userName: 'davilam',
      apellidos: 'Ávila Macias',
      puesto: 'Ingeniero de Desarrollo',
      departamento: 'TI - APLICACIONES',
      fechaIngreso: new Date(),
      antiguedad: null,
      empresa: 'HOLA',
      telefono: '70515',
      mobile: '',
      mail: 'davilam@ho1a.com',
      jefeUserName: 'adominguezc',
      isJefe: false,
      nivelCompetencia: 3,
      oficina: 'GUADALAJARA',
      showExpediente: false,
      authenticated: true,
      showNew: false,
      showSearch: false,
      showCatalog: false,
      foto: ''
    };
  }

  getMenuMock(showExpediente: boolean, showNew: boolean = false, showSearch: boolean = false, showCatalog: boolean = false): Menu[] {
    let menus: Menu[];
    menus = [
      {
        name: 'Menú de navegación', specialPermission: false,
        routes: [
          { name: 'Inicio', route: 'mirh/home', icon: 'home' }
        ]
      },
      {
        name: 'Menú de administrador', specialPermission: false,
        routes: [
          { name: 'Solicitudes', route: 'mirh/', icon: 'dashboard' },
          { name: 'Candidatos', route: 'mirh/', icon: 'business_center' },
          { name: 'Alta de colaborador', route: 'mirh/', icon: 'group_add' }
        ]
      }
    ];
    if (showExpediente) {
      menus[0].routes.push({ name: 'Expedientes', route: 'mirh/solicitud/files', icon: 'attachment' });
    }
    if (showNew) {
      menus[0].routes.push({ name: 'Nueva solicitud de plaza', route: 'mirh/solicitud', icon: 'library_books' });
    }
    if (showSearch) {
      menus[0].routes.push({ name: 'Solicitudes de plaza', route: 'mirh/solicitud/search', icon: 'dashboard' });
    }
    if (showCatalog) {
      menus[0].routes.push({ name: 'Alta de colaborador', route: 'mirh/catalog/candidatos', icon: 'dashboard' });
    }
    return menus;
  }
}
