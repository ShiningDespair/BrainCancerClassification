import { Suspense } from 'react';
import DashboardContent from './DashboardContent';

export default function Page() {
  return (
    <Suspense fallback={<div>Yükleniyor...</div>}>
      <DashboardContent />
    </Suspense>
  );
}