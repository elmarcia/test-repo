import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { GendecDocumentDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-gendec-preview',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './gendec-preview.component.html',
  styleUrl: './gendec-preview.component.css'
})
export class GendecPreviewComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly operationalApi = inject(OperationalApiService);

  document: GendecDocumentDto | null = null;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    const flightId = this.route.snapshot.paramMap.get('flightId');
    if (!flightId) {
      this.errorMessage = 'Flight ID is required to preview GENDEC.';
      this.isLoading = false;
      return;
    }

    this.operationalApi.getGendec(flightId).subscribe({
      next: (document) => {
        this.document = document;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'GENDEC preview is unavailable for this flight.';
        this.isLoading = false;
      }
    });
  }

  print(): void {
    window.print();
  }
}
