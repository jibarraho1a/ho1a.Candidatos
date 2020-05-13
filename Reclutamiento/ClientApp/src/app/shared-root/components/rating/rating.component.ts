import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';

@Component({
  selector: 'ho1a-rating',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.scss']
})
export class RatingComponent implements OnInit, OnChanges {

  @Input() rating = 0;
  @Input() maxRating = 0;
  @Input() readOnly = false;

  @Output() ratingClicked: EventEmitter<number> = new EventEmitter<number>();

  ratings: any[];

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(): void {
    this.setRatings();
  }

  onClick(value: number): void {
    if (!this.readOnly) {
      this.rating = value;
      this.ratingClicked.emit(value);
      this.setRatings();
    }
  }

  setRatings() {
    this.ratings = Array(this.maxRating).fill(this.rating).map((x, i) => {
      const index = this.maxRating - i;
      const checked = index > x ? false : true;
      return { checked: checked, value: index };
    });
  }

}
