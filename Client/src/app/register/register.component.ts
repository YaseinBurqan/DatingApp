import { AccountService } from './../_services/account.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegistration = new EventEmitter();

  model: any = {};

  constructor(private accountService: AccountService) {}

  ngOnInit() {}

  register() {
    this.accountService.register(this.model).subscribe({
      next: () => this.cancel(),
      error: (error) => console.log(error),
    });
  }

  cancel() {
    this.cancelRegistration.emit(false);
  }
}
