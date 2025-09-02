# prediction/urls.py

from django.urls import path
# Import both of your views
from .views import PredictView, HealthCheckView

urlpatterns = [
    # This is your existing URL for making predictions
    path('predict/<int:model_id>/', PredictView.as_view(), name='predict'),

    # ⬇️ This is the new URL for the health check ⬇️
    path('health/', HealthCheckView.as_view(), name='health_check'),
]