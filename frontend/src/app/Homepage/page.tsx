import { Suspense } from 'react';
import HomepageContent from './HomepageContent';

export default function Page() {
  return (
    <Suspense fallback={<div>Yükleniyor...</div>}>
      <HomepageContent />
    </Suspense>
  );
}