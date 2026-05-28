import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./modules/dashboard/dashboard.component').then((component) => component.DashboardComponent)
  },
  {
    path: 'timeline',
    loadComponent: () =>
      import('./modules/flight-timeline/flight-timeline.component').then((component) => component.FlightTimelineComponent)
  },
  {
    path: 'documents/gendec',
    loadComponent: () =>
      import('./modules/gendec-document/gendec-document.component').then((component) => component.GendecDocumentComponent)
  },
  {
    path: 'documents/crew-manifest',
    loadComponent: () =>
      import('./modules/crew-manifest-document/crew-manifest-document.component').then((component) => component.CrewManifestDocumentComponent)
  },
  {
    path: 'documents',
    loadComponent: () =>
      import('./modules/documents-hub/documents-hub.component').then((component) => component.DocumentsHubComponent)
  },
  {
    path: 'reports/disruption-summary',
    loadComponent: () =>
      import('./modules/disruption-summary/disruption-summary.component').then((component) => component.DisruptionSummaryComponent)
  },
  {
    path: 'reports/recovery-actions',
    loadComponent: () =>
      import('./modules/recovery-actions-report/recovery-actions-report.component').then((component) => component.RecoveryActionsReportComponent)
  },
  {
    path: 'gendec/:flightId',
    loadComponent: () =>
      import('./modules/gendec-preview/gendec-preview.component').then((component) => component.GendecPreviewComponent)
  },
  {
    path: 'crew-manifest/:flightId',
    loadComponent: () =>
      import('./modules/crew-manifest-preview/crew-manifest-preview.component').then((component) => component.CrewManifestPreviewComponent)
  },
  {
    path: 'reports',
    loadComponent: () =>
      import('./modules/reports-hub/reports-hub.component').then((component) => component.ReportsHubComponent)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
