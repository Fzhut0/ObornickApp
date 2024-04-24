import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Link } from 'src/app/_models/link';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';

@Component({
  selector: 'app-delete-link',
  templateUrl: './delete-link.component.html',
  styleUrls: ['./delete-link.component.css']
})
export class DeleteLinkComponent implements OnInit {
  link: Link | null = null;

  constructor(public bsModalRef: BsModalRef, private linkService: LinksService, private categoryService: CategoryService) {}

  ngOnInit(): void {
    
  }

  removeLink()
  {
    if (!this.link)
    {
      return;
      }
    this.linkService.deleteLink(this.link.customName).subscribe({
      next: () => {
        this.bsModalRef.hide()
      }
    })
  }
}
