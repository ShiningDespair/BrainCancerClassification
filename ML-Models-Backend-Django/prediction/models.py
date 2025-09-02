# This is an auto-generated Django model module.
# You'll have to do the following manually to clean this up:
#   * Rearrange models' order
#   * Make sure each model has one field with primary_key=True
#   * Make sure each ForeignKey and OneToOneField has `on_delete` set to the desired behavior
#   * Remove `managed = False` lines if you wish to allow Django to create, modify, and delete the table
# Feel free to rename the models, but don't rename db_table values or field names.
from django.db import models


class Diseases(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    name = models.CharField(db_column='Name', max_length=100, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    description = models.CharField(db_column='Description', max_length=500, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'Diseases'


class Predictionmodels(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    modelname = models.CharField(db_column='ModelName', max_length=100, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    version = models.CharField(db_column='Version', max_length=20, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    description = models.TextField(db_column='Description', db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    filepath = models.CharField(db_column='FilePath', max_length=500, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    framework = models.CharField(db_column='Framework', max_length=50, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    inputshape = models.CharField(db_column='InputShape', max_length=50, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    numclasses = models.IntegerField(db_column='NumClasses')  # Field name made lowercase.
    labels = models.CharField(db_column='Labels', max_length=200, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    createdat = models.DateTimeField(db_column='CreatedAt')  # Field name made lowercase.
    accuracy = models.DecimalField(db_column='Accuracy', max_digits=5, decimal_places=2, blank=True, null=True)  # Field name made lowercase.
    toolid = models.ForeignKey('Tools', models.DO_NOTHING, db_column='ToolId', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'PredictionModels'


class Predictions(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    uploadid = models.ForeignKey('Uploads', models.DO_NOTHING, db_column='UploadId')  # Field name made lowercase.
    modelid = models.ForeignKey(Predictionmodels, models.DO_NOTHING, db_column='ModelId')  # Field name made lowercase.
    diseaseid = models.ForeignKey(Diseases, models.DO_NOTHING, db_column='DiseaseId', blank=True, null=True)  # Field name made lowercase.
    predictionresult = models.CharField(db_column='PredictionResult', max_length=100, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    confidence = models.DecimalField(db_column='Confidence', max_digits=5, decimal_places=4, blank=True, null=True)  # Field name made lowercase.
    predictedat = models.DateTimeField(db_column='PredictedAt')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'Predictions'


class Roles(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    rolename = models.CharField(db_column='RoleName', unique=True, max_length=50, db_collation='Turkish_CI_AS')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'Roles'


class Toolimages(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    toolid = models.ForeignKey('Tools', models.DO_NOTHING, db_column='ToolId')  # Field name made lowercase.
    src = models.CharField(db_column='Src', max_length=500, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    alt = models.CharField(db_column='Alt', max_length=200, db_collation='Turkish_CI_AS')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'ToolImages'


class Tools(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    icon = models.CharField(db_column='Icon', max_length=100, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    title = models.CharField(db_column='Title', max_length=100, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    description = models.TextField(db_column='Description', db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    href = models.CharField(db_column='Href', max_length=200, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'Tools'


class Uploads(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    userid = models.ForeignKey('Users', models.DO_NOTHING, db_column='UserId')  # Field name made lowercase.
    filepath = models.CharField(db_column='FilePath', max_length=500, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    filetype = models.CharField(db_column='FileType', max_length=20, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    uploadedat = models.DateTimeField(db_column='UploadedAt')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'Uploads'


class Users(models.Model):
    id = models.AutoField(db_column='Id', primary_key=True)  # Field name made lowercase.
    clerkid = models.CharField(db_column='ClerkId', unique=True, max_length=100, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    email = models.CharField(db_column='Email', unique=True, max_length=100, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    firstname = models.CharField(db_column='FirstName', max_length=50, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    lastname = models.CharField(db_column='LastName', max_length=50, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    profileimageurl = models.CharField(db_column='ProfileImageUrl', max_length=500, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    createdat = models.DateTimeField(db_column='CreatedAt')  # Field name made lowercase.
    updatedat = models.DateTimeField(db_column='UpdatedAt')  # Field name made lowercase.
    lastloginat = models.DateTimeField(db_column='LastLoginAt', blank=True, null=True)  # Field name made lowercase.
    isactive = models.BooleanField(db_column='IsActive')  # Field name made lowercase.
    phonenumber = models.CharField(db_column='PhoneNumber', max_length=20, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    dateofbirth = models.DateField(db_column='DateOfBirth', blank=True, null=True)  # Field name made lowercase.
    gender = models.CharField(db_column='Gender', max_length=10, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    country = models.CharField(db_column='Country', max_length=50, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    city = models.CharField(db_column='City', max_length=50, db_collation='Turkish_CI_AS', blank=True, null=True)  # Field name made lowercase.
    roleid = models.ForeignKey(Roles, models.DO_NOTHING, db_column='RoleId')  # Field name made lowercase.
    isemailverified = models.BooleanField(db_column='IsEmailVerified')  # Field name made lowercase.
    preferredlanguage = models.CharField(db_column='PreferredLanguage', max_length=5, db_collation='Turkish_CI_AS')  # Field name made lowercase.
    createdby = models.ForeignKey('self', models.DO_NOTHING, db_column='CreatedBy', blank=True, null=True)  # Field name made lowercase.
    updatedby = models.ForeignKey('self', models.DO_NOTHING, db_column='UpdatedBy', related_name='users_updatedby_set', blank=True, null=True)  # Field name made lowercase.
    deletedat = models.DateTimeField(db_column='DeletedAt', blank=True, null=True)  # Field name made lowercase.
    deletedby = models.ForeignKey('self', models.DO_NOTHING, db_column='DeletedBy', related_name='users_deletedby_set', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'Users'
