export interface Combo {
  disabled: boolean | null;
  group: { disabled: boolean; name: string } | null;
  selected: boolean | null;
  text: string;
  value: number | string | null;
}
