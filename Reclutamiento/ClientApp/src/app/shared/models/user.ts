export interface User extends Complement {
  id: number;
  nombre: string | null;
  userName: string | null;
  apellidos: string | null;
  puesto: string | null;
  departamento: string | null;
  fechaIngreso: Date | null;
  antiguedad: number | null;
  empresa: string | null;
  telefono: string | null;
  mobile: string | null;
  mail: string | null;
  jefeUserName: string | null;
  isJefe: boolean | null;
  authenticated: boolean;
  foto: any | null;
  nivelCompetencia: number | null;
  oficina: string | null;
  isAdmin?: boolean | null;
  equipo?: string | null;
  area?: string | null;
  direccion?: string | null;
  nivelOrganizacional?: string | null;
  showExpediente?: boolean | null;
  showNew?: boolean | null;
  showSearch?: boolean | null;
  showCatalog?: boolean | null;
  token?: string | null;
}

interface Complement {
  puestoSolicitado?: string | null;
}
