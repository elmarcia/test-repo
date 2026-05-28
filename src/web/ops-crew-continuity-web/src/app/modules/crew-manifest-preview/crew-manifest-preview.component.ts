import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { CrewManifestDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-crew-manifest-preview',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './crew-manifest-preview.component.html',
  styleUrl: './crew-manifest-preview.component.css'
})
export class CrewManifestPreviewComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly operationalApi = inject(OperationalApiService);

  document: CrewManifestDto | null = null;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    const flightId = this.route.snapshot.paramMap.get('flightId');
    if (!flightId) {
      this.errorMessage = 'Flight ID is required to preview a crew manifest.';
      this.isLoading = false;
      return;
    }

    this.operationalApi.getCrewManifest(flightId).subscribe({
      next: (document) => {
        this.document = document;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Crew Manifest preview is unavailable for this flight.';
        this.isLoading = false;
      }
    });
  }

  print(): void {
    window.print();
  }
}
