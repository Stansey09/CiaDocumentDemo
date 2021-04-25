import { Component, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AppService } from './app.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnDestroy {

  constructor(private appService: AppService) {}

  title = 'angular-nodejs-example';

  userForm = new FormGroup({
    documentTitle: new FormControl('', Validators.nullValidator && Validators.required),
    documentText: new FormControl('', Validators.nullValidator && Validators.required)
  });

  redactedForm = new FormGroup({
    redactedTextField: new FormControl('')
  })

  users: any[] = [];

  destroy$: Subject<boolean> = new Subject<boolean>();

  onSubmit() {

    this.appService.addUser(this.userForm.value).pipe(takeUntil(this.destroy$)).subscribe(data => {
      console.log('message::::', data);
      this.userForm.reset();
    });
  }

  getAllUsers() {
    this.appService.getUsers().pipe(takeUntil(this.destroy$)).subscribe((users: any[]) => {
        this.users = users;
    });
  }

  getRedactedText(title){
     this.appService.getRedactedByTitle(title).pipe(takeUntil(this.destroy$)).subscribe(data => {
       console.log('message::::', data);
       this.redactedForm.get("redactedTextField").setValue(JSON.stringify(data));
       var jsonObject = JSON.parse(JSON.stringify(data));
       //jsonObject.documentTitle
       console.log(jsonObject.documentText);
       this.redactedForm.get("redactedTextField").setValue(jsonObject.documentText);
       //this.redactedTextField.setValue(data);
       //gotta put this stuff somewhere
     })
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
