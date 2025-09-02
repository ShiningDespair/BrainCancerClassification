# prediction/serializers.py

from rest_framework import serializers

class PredictionSerializer(serializers.Serializer):
    image = serializers.ImageField()