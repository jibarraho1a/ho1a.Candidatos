import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TokenService } from './token.service';

import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/filter';
import 'rxjs/add/observable/of';

import { User } from '../../shared/models/user';
import { Store } from '../../shared/class/Store';
import { StateOption } from '../../shared/enums/state-option.enum';
import { TypeFilter } from '../../shared/enums/type-filter.enum';

const API = environment.apiReclutamiento;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    private userSubject: BehaviorSubject<User>;
    public user: Observable<User>;

    constructor(private store: Store,
                private http: HttpClient,
                private tokenService: TokenService) {
        this.userSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('user')));
        this.user = this.userSubject.asObservable();
    }

    public get userValue(): User {
        return this.userSubject.value;
    }

    public setUserValue(user: User) {
      this.userSubject.next(user);
    }

    get currentUser(): User {
      let user = JSON.parse(localStorage.getItem('user'));
      const filters = this.store.value[StateOption.filters];
      if (filters && filters[TypeFilter.chief]) {
        user = filters[TypeFilter.chief].value;
      }
      return user;
    }

    get currentUserName(): string {
      let user;
      const filters = this.store.value[StateOption.filters];
      if (filters && filters[TypeFilter.chief] && filters[TypeFilter.chief].value) {
        user = filters[TypeFilter.chief].value;
      } else {
        user = JSON.parse(localStorage.getItem('user')).userName;
      }
      return user;
    }

    loginUser(username) {
      this.logoutUser();
      return this.http.post<User>(`${API}/login/authenticate`, { username } )
        .do( (next: User) => {
          if (!next) {
            localStorage.setItem('user', null);
            this.store.set('user', null);
            return;
          }
          const user: User = <User>{
            id: next.id,
            nombre: next.nombre,
            userName: next.userName,
            apellidos: next.apellidos,
            puesto: next.puesto,
            departamento: next.departamento,
            fechaIngreso: next.fechaIngreso,
            antiguedad: next.antiguedad,
            empresa: next.empresa,
            telefono: next.telefono,
            mobile: next.mobile,
            mail: next.mail,
            jefeUserName: next.jefeUserName,
            isJefe: next.isJefe,
            foto: next.foto,
            nivelCompetencia: next.nivelCompetencia,
            oficina: next.oficina,
            isAdmin: true,
            showExpediente: true,
            authenticated: true,
            showNew: true,
            showSearch: true,
            showCatalog: true,
            puestoSolicitado: next.puestoSolicitado,
          };

          localStorage.setItem('user', JSON.stringify(user));
          this.tokenService.setToken(next.token);
          this.store.set('user', user);
        })
        .map( data => {
          const user = this.store.value['user'];
          if (user !== null && user !== undefined && user.authenticated) {
            return data;
          } else {
            return null;
          }
        } );
    }

    logoutUser() {
        localStorage.clear();
        this.userSubject.next(null);
    }

    getChiefs() {
      const user = JSON.parse(localStorage.getItem('user'));
      return this.http.get<any[]>(`${API}/${user.userName}/colaboradoresJefesACargo`)
        .do(next => this.store.set(StateOption.chiefs, next));
    }

}
