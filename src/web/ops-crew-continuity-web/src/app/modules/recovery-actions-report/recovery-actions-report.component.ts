import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';

import { RecoveryActionsReportDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-recovery-actions-report',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './recovery-actions-report.component.html',
  styleUrl: './recovery-actions-report.component.css'
})
export class RecoveryActionsReportComponent implements OnInit {
  private readonly operationalApi = inject(OperationalApiService);

  report: RecoveryActionsReportDto | null = null;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.operationalApi.getRecoveryActionsReport().subscribe({
      next: (report) => {
        this.report = report;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Recovery Actions Report is unavailable.';
        this.isLoading = false;
      }
    });
  }

  print(): void {
    window.print();
  }
}
