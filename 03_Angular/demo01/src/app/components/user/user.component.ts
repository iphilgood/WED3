import { Component, OnInit } from '@angular/core';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  name = 'Steve Smith';
  age: number;
  email: string;
  address: Address;
  hobbies: string[];
  posts: Post[];
  isEdit = false;

  constructor(private dataService: DataService) {
    console.log('constructor ran ...');
   }

  ngOnInit() {
    this.name = 'John Doe';
    this.age = 30;
    this.email = 'john.doe@example.com';
    this.address = {
      street: '50 Main st',
      city: 'Boston',
      state: 'Ma'
    };
    this.hobbies = ['Write code', 'Watch movies', 'Liste to music'];

    this.dataService.getPosts().subscribe((posts) => {
      this.posts = posts;
    });
  }

  onClick() {
    this.name = 'Phil Schilter';
    this.hobbies.push('New hobby');
  }

  addHobby(hobby: string) {
    console.log(hobby);
    this.hobbies.unshift(hobby);

    return false;
  }

  deleteHobby(hobby: string) {
    for (let i = 0; i < this.hobbies.length; i++) {
      if (this.hobbies[i] === hobby) {
        this.hobbies.splice(i, 1);
      }
    }
  }

  toggleEdit() {
    this.isEdit = !this.isEdit;
  }
}

interface Address {
  street: string;
  city: string;
  state: string;
}

interface Post {
  id: number;
  title: string;
  body: string;
  userId: number;
}
