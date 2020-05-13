import { State, Selector, Action, StateContext } from "@ngxs/store";
import { Notificacion } from "src/app/shared/models/notificacion";

export class NotificacionesStateModel {
    notificaciones: Notificacion[];
}

@State<NotificacionesStateModel>({
    name: 'notificaciones',
    defaults: {
        notificaciones: [{
            titulo: "Una de tus solicitudes ha avanzado",
            contenido: "A321: ha sido seleccionado para contratación"
        },
        {
            titulo: "Tienes solicitudes pendientes",
            contenido: "Hay solicitudes que necesitan tu aprobación"
        },
        {
            titulo: "Lorem ipsum dolor sit amet",
            contenido: "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo"
        }],
    }
})

export class NotificacionState {

    @Selector()
    static notificaciones(state: NotificacionesStateModel) {
        return state.notificaciones;
    }
}