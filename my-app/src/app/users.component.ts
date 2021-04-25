import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  constructor() { }

  @Input() users: any[];
  @Output() getRedactedText = new EventEmitter<string>();

  ngOnInit(): void {
  }

  _getRedactedText(data:string){
      this.getRedactedText.emit(data);
  }

}
