import { MenuRoute } from './menu-route';

export interface Menu {
    name: string;
    specialPermission: boolean;
    routes: MenuRoute[];
}
