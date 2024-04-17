import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: User[] = [];
  newFacebookId: string = '';
  selectedUsername: string = '';

  constructor(private adminService: AdminService, private modalService: BsModalService) {
    
   }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers()
  {
    this.adminService.getUsers().subscribe({
      next: (response: User[]) => {
        this.users = response,
          console.log(this.users)
      }
    });
  }

  updateUserFacebookId(): void {
    console.log(this.selectedUsername, this.newFacebookId)
    if (this.newFacebookId && this.selectedUsername) {
      this.adminService.updateUserFacebookId(this.newFacebookId, this.selectedUsername).subscribe(() => {
        this.getUsers();
      });
    }
  }

  clearUserFacebookId(): void {
    this.adminService.clearUserFacebookId(this.selectedUsername).subscribe(() => {
      this.getUsers();
    });
  }

  selectUser(userName: string): void {
    this.selectedUsername = userName;
    console.log(this.selectedUsername);
  }

}