import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterDetailCandidate'
})
export class FilterDetailCandidatePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    return null;
  }

}
