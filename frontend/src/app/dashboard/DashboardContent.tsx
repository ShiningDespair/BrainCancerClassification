import { currentUser } from '@clerk/nextjs/server';
import { redirect } from 'next/navigation';

const DashboardPage = async () => {
  const user = await currentUser();

  if (!user) {
    redirect('/sign-in');
  }

  return (
    <div className="min-h-screen bg-primary p-8 pt-24"> {/* pt-24 fixed header için */}
      <div className="max-w-4xl mx-auto">
        <h1 className="text-3xl font-bold text-white mb-8">
          Dashboard
        </h1>
        <div className="bg-black/30 rounded-lg p-6 backdrop-blur-sm border border-white/10">
          <p className="text-slate-300 mb-4">
            Hoş geldin, {user.firstName} {user.lastName}!
          </p>
          <p className="text-slate-400">
            Bu sayfa sadece giriş yapmış kullanıcılar tarafından görülebilir.
          </p>
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;