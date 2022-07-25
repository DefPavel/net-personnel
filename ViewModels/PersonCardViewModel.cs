
using AlphaPersonel.Models.Home;
using AlphaPersonel.ViewModels.Reports;
using AlphaPersonel.Views.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;

namespace AlphaPersonel.ViewModels;

internal class PersonCardViewModel : BaseViewModel
{
    public PersonCardViewModel(Users users, NavigationStore navigationStore, Persons person, int idDepartment)
    {
        _navigationStore = navigationStore;
        _selectedPerson = person;
        _idDepartment = idDepartment;
        _user = users;
    }

    #region Стаж

    // Стаж общий
    private string _stageIsOver = string.Empty;
    public string StageIsOver
    {
        get => _stageIsOver;
        private set => Set(ref _stageIsOver, value);
    }

    private string _urlDocument = string.Empty;
    public string UrlDocument
    {
        get => _urlDocument;
        set => Set(ref _urlDocument, value);
    }
    // Стаж в Универе
    private string _stageIsUniver = string.Empty;
    public string StageIsUniver
    {
        get => _stageIsUniver;
        private set => Set(ref _stageIsUniver, value);
    }

    // Стаж Научный
    private string _stageIsScience = string.Empty;
    public string StageIsScience
    {
        get => _stageIsScience;
        private set => Set(ref _stageIsScience, value);
    }

    // Стаж Научно-Педагогический
    private string _stageIsPedagogical = string.Empty;
    public string StageIsPedagogical
    {
        get => _stageIsPedagogical;
        private set => Set(ref _stageIsPedagogical, value);
    }

    // Стаж Медицинский
    private string _stageIsMedical = string.Empty;
    public string StageIsMedical
    {
        get => _stageIsMedical;
        private set => Set(ref _stageIsMedical, value);
    }

    // Стаж Музея
    private string _stageIsMuseum = string.Empty;
    public string StageIsMuseum
    {
        get => _stageIsMuseum;
        private set => Set(ref _stageIsMuseum, value);
    }

    // Стаж Библиотека
    private string _stageIsLibrary = string.Empty;
    public string StageIsLibrary
    {
        get => _stageIsLibrary;
        private set => Set(ref _stageIsLibrary, value);
    }

    #endregion

    #region Переменные

    private readonly int _idDepartment;

    private readonly NavigationStore _navigationStore;

    private Users? _user;
    public Users? User
    {
        get => _user;
        set => Set(ref _user, value);

    }
    private bool _radioIsMale;
    public bool RadioIsMale
    {
        get => _radioIsMale;
        set => Set(ref _radioIsMale, value);
    }

    private bool _radioIsFemale;
    public bool RadioIsFemale
    {
        get => _radioIsFemale;
        set => Set(ref _radioIsFemale, value);
    }

    private Pensioner? _selectedPens;
    public Pensioner? SelectedPens
    {
        get => _selectedPens;
        set => Set(ref _selectedPens, value);

    }

    private Family? _selectedFamily;
    public Family? SelectedFamily
    {
        get => _selectedFamily;
        set => Set(ref _selectedFamily, value);

    }

    private Position? _selectedPosition;
    public Position? SelectedPosition
    {
        get => _selectedPosition;
        set => Set(ref _selectedPosition, value);

    }
    private Invalid? _selectedInvalid;
    public Invalid? SelectedInvalid
    {
        get => _selectedInvalid;
        set => Set(ref _selectedInvalid, value);

    }

    private Education? _selectedEducation;
    public Education? SelectedEducation
    {
        get => _selectedEducation;
        set => Set(ref _selectedEducation, value);

    }

    private OldSurname? _selectedOldSurname;
    public OldSurname? SelectedOldSurname
    {
        get => _selectedOldSurname;
        set => Set(ref _selectedOldSurname, value);
    }

    private Rewarding? _selectedRewarding;
    public Rewarding? SelectedRewarding
    {
        get => _selectedRewarding;
        set => Set(ref _selectedRewarding, value);
    }
    private Move? _selectedMove;
    public Move? SelectedMove
    {
        get => _selectedMove;
        set => Set(ref _selectedMove, value);
    }
    
    private Documents? _selectedDocument;
    public Documents? SelectedDocument
    {
        get => _selectedDocument;
        set
        {
            Set(ref _selectedDocument, value);
            if (_selectedDocument != null)
            {
                UrlDocument = _selectedDocument.Url; 
            }
            else
            {
                UrlDocument = string.Empty;
            }
        }
    }

    private VisualBoolean? _isLoading;

    public VisualBoolean? IsLoading
    {
        get => _isLoading;
        set => Set(ref _isLoading, value);

    }

    private int _indexPerson;

    public int IndexPerson
    {
        get => _indexPerson;
        set => Set(ref _indexPerson, value);
    }

    // Поисковая строка для поиска по ФИО сотрудника
    private string? _filterPerson;
    public string? FilterPerson
    {
        get => _filterPerson;
        set
        {
            _ = Set(ref _filterPerson, value);
            if (PersonsList != null)
            {
                CollectionPerson!.Refresh();
            }
        }
    }
    // Специальная колекция для фильтров
    private ICollectionView? _collectionPerson;
    public ICollectionView? CollectionPerson
    {
        get => _collectionPerson;
        private set => Set(ref _collectionPerson, value);
    }
    // Список людей которые тоже находятся в данном отделе
    private IEnumerable<Persons>? _personsList;
    public IEnumerable<Persons>? PersonsList
    {
        get => _personsList;
        private set
        {
            _ = Set(ref _personsList, value);
            // Для фильтрации полей 
            if (PersonsList != null) CollectionPerson = CollectionViewSource.GetDefaultView(PersonsList);

            if (CollectionPerson != null)
            { 
                CollectionPerson.Filter = FilterToPerson;
               // CollectionPerson.MoveCurrentToFirst();
                //SelectedPerson = CollectionPerson.SourceCollection;
            }
        }
    }
    private IEnumerable<Departments>? _departments;
    public IEnumerable<Departments>? Departments
    {
        get => _departments;
        private set => Set(ref _departments, value);
    }
    private IEnumerable<TypePosition>? _typePositions;
    public IEnumerable<TypePosition>? TypePositions
    {
        get => _typePositions;
        private set => Set(ref _typePositions, value);
    }
    
    // Тип контракта
    private IEnumerable<TypeContract>? _typeContracts;
    public IEnumerable<TypeContract>? TypeContracts
    {
        get => _typeContracts;
        private set => Set(ref _typeContracts, value);
    }
    // Место работы
    private IEnumerable<PlaceOfWork>? _typePlaceOfWorks;
    public IEnumerable<PlaceOfWork>? TypePlaceOfWorks
    {
        get => _typePlaceOfWorks;
        private set => Set(ref _typePlaceOfWorks, value);
    }
    
    // Список видов паспортов
    private IEnumerable<TypePassport>? _typePassports;
    public IEnumerable<TypePassport>? TypePassports
    {
        get => _typePassports;
        private set => Set(ref _typePassports, value);
    }
    // Тип документов
    private IEnumerable<TypeDocuments>? _typeDocuments;
    public IEnumerable<TypeDocuments>? TypeDocuments
    {
        get => _typeDocuments;
        private set => Set(ref _typeDocuments, value);
    }


    // Список период отпусков
    private IEnumerable<PeriodVacation>? _periodVacation;
    public IEnumerable<PeriodVacation>? PeriodVacation
    {
        get => _periodVacation;
        private set => Set(ref _periodVacation, value);
    }

    // Список тип отпусков
    private IEnumerable<Models.TypeVacation>? _typeVacation;
    public IEnumerable<Models.TypeVacation>? TypeVacation
    {
        get => _typeVacation;
        private set => Set(ref _typeVacation, value);
    }

    // Список тип родства
    private IEnumerable<TypeFamily>? _typeFamily;
    public IEnumerable<TypeFamily>? TypeFamily
    {
        get => _typeFamily;
        private set => Set(ref _typeFamily, value);
    }

    // Список тип пенсионера 
    private IEnumerable<TypePensioner>? _typePensioner;
    public IEnumerable<TypePensioner>? TypePensioner
    {
        get => _typePensioner;
        private set => Set(ref _typePensioner, value);
    }
    // Список тип образования 
    private IEnumerable<TypeEducation>? _typeEducation;
    public IEnumerable<TypeEducation>? TypeEducation
    {
        get => _typeEducation;
        private set => Set(ref _typeEducation, value);
    }
    // тип награждения
    private IEnumerable<Rewarding>? _typeRewarding;
    public IEnumerable<Rewarding>? TypeRewarding
    {
        get => _typeRewarding;
        private set => Set(ref _typeRewarding, value);
    }
    // Приказы для награждения
    private IEnumerable<Rewarding>? _orderRewarding;
    public IEnumerable<Rewarding>? OrderRewarding
    {
        get => _orderRewarding;
        private set => Set(ref _orderRewarding, value);
    }
    // Приказы Отпусков
    private IEnumerable<Vacation>? _orderVacations;
    public IEnumerable<Vacation>? OrderVacations
    {
        get => _orderVacations;
        private set => Set(ref _orderVacations, value);
    }

    // Приказы для смены фамилии
    private IEnumerable<Order>? _orderOldSurname;
    public IEnumerable<Order>? OrderOldSurname
    {
        get => _orderOldSurname;
        private set => Set(ref _orderOldSurname, value);
    }
    // мед.категории
    private IEnumerable<MedicalCategory>? _medicalCategory;
    public IEnumerable<MedicalCategory>? MedicalCategory
    {
        get => _medicalCategory;
        private set => Set(ref _medicalCategory, value);
    }

    private IEnumerable<TypeDegree>? _typeDegree;
    public IEnumerable<TypeDegree>? TypeDegree
    {
        get => _typeDegree;
        private set => Set(ref _typeDegree, value);
    }

    // Ученая степень
    private IEnumerable<ScientificDegree>? _scientificDegree;
    public IEnumerable<ScientificDegree>? ScientificDegree
    {
        get => _scientificDegree;
        set => Set(ref _scientificDegree, value);
    }

    // Академик- Член.корр.
    private IEnumerable<TypeRank>? _typeRanks;
    public IEnumerable<TypeRank>? TypeRanks
    {
        get => _typeRanks;
        private set => Set(ref _typeRanks, value);
    }
    // Справочник для ученого звания
    private IEnumerable<TypeRank>? _typeTitle;
    public IEnumerable<TypeRank>? TypeTitle
    {
        get => _typeTitle;
        private set => Set(ref _typeTitle, value);
    }

    // Передать значение Отдела
    private Departments? _department;
    public Departments? Department
    {
        get => _department;
        set => Set(ref _department, value);
    }

    // Выбранный отпуск 
    private Vacation? _selectedVacation;
    public Vacation? SelectedVacation
    {
        get => _selectedVacation;
        set => Set(ref _selectedVacation, value);
    }
    // Выбранный стаж
    private HistoryEmployment? _selectedHistory;
    public HistoryEmployment? SelectedHistory
    {
        get => _selectedHistory;
        set => Set(ref _selectedHistory, value);
    }

    private Qualification? _selectedQualification;
    public Qualification? SelectedQualification
    {
        get => _selectedQualification;
        set => Set(ref _selectedQualification, value);
    }

    private Medical? _selectedMedical;
    public Medical? SelectedMedical
    {
        get => _selectedMedical;
        set => Set(ref _selectedMedical, value);
    }

    private MemberAcademic? _seletedMemberAcademic;
    public MemberAcademic? SeletedMemberAcademic
    {
        get => _seletedMemberAcademic;
        set => Set(ref _seletedMemberAcademic, value);
    }
    private AcademicTitle? _selectedTitle;
    public AcademicTitle? SelectedTitle
    {
        get => _selectedTitle;
        set => Set(ref _selectedTitle, value);
    }

    private ScientificDegree? _seletedDegree;
    public ScientificDegree? SeletedDegree
    {
        get => _seletedDegree;
        set => Set(ref _seletedDegree, value);
    }
    
    // Выбранная персона
    private Persons? _selectedPerson;
    public Persons? SelectedPerson
    {
        get => _selectedPerson;
        set
        {
            Set(ref _selectedPerson, value);
            if (_selectedPerson == null) return;
            if (_selectedPerson.Gender == "male")
            {
                RadioIsMale = true;
            }
            else
            {
                RadioIsFemale = true;
            }
            // SaveMainInfoByPerson(_selectedPerson);
            
        }
    }
    #endregion

    #region Команды

    #region Menu Items

    private ICommand? _logout;
    public ICommand Logout => _logout ??= new LambdaCommand(Exit);

    private ICommand? _openMasterDrop;
    public ICommand OpenMasterDrop => _openMasterDrop ??= new LambdaCommand(OpenMasterDropView);

    private ICommand? _openDepartment;
    public ICommand OpenDepartment => _openDepartment ??= new LambdaCommand(OpenDepartmentView);

    private ICommand? _openTypeVacation;
    public ICommand OpenVacation => _openTypeVacation ??= new LambdaCommand(OpenTypeVacationView);

    private ICommand? _openTypeRewarding;
    public ICommand OpenRewarding => _openTypeRewarding ??= new LambdaCommand(OpenTypeRewardingView);

    private ICommand? _openSearch;
    public ICommand OpenSearch => _openSearch ??= new LambdaCommand(OpenSearchView);

    private ICommand? _openPosition;
    public ICommand OpenPosition => _openPosition ??= new LambdaCommand(OpenPositionView);

    private ICommand? _openTypePosition;
    public ICommand OpenTypePosition => _openTypePosition ??= new LambdaCommand(OpenTypePositionView);

    private ICommand? _openTypeOrder;
    public ICommand OpenTypeOrder => _openTypeOrder ??= new LambdaCommand(OpenTypeOrderView);

    private ICommand? _openTypeRanks;
    public ICommand OpenTypeRanks => _openTypeRanks ??= new LambdaCommand(OpenTypeRankView);

    private ICommand? _openOrder;
    public ICommand OpenOrder => _openOrder ??= new LambdaCommand(OpenOrderView);

    private ICommand? _openPeriod;
    public ICommand OpenPeriod => _openPeriod ??= new LambdaCommand(OpenPeriodView);

    private ICommand? _openReport;
    public ICommand OpenReport => _openReport ??= new LambdaCommand(OpenReportView);

    private ICommand? _openReportInsert;
    public ICommand OpenReportInsert => _openReportInsert ??= new LambdaCommand(OpenReportInsertPerson);

    private ICommand? _openReportInsertOther;
    public ICommand OpenReportInsertOther => _openReportInsertOther ??= new LambdaCommand(OpenReportInsertPersonIsOther);

    private ICommand? _openReportDelete;
    public ICommand OpenReportDelete => _openReportDelete ??= new LambdaCommand(OpenReportDropPerson);

    private ICommand? _openReportDeleteOther;
    public ICommand OpenReportDeleteOther => _openReportDeleteOther ??= new LambdaCommand(OpenReportDropPersonIsOther);


    private ICommand? _openReportRewarding;
    public ICommand OpenReportRewarding => _openReportRewarding ??= new LambdaCommand(OpenReportRewardingPerson);

    private ICommand? _openReportVacation;
    public ICommand OpenReportVacation => _openReportVacation ??= new LambdaCommand(OpenReportVacationPeriod);

    private ICommand? _openReportContract;
    public ICommand OpenReportContract => _openReportContract ??= new LambdaCommand(OpenReportContracts);

    private ICommand? _openMasterReport;
    public ICommand OpenMasterReport => _openMasterReport ??= new LambdaCommand(OpenMasterReportView);

    private ICommand? _openReportContractIsPluralism;
    public ICommand OpenReportContractIsPluralism => _openReportContractIsPluralism ??= new LambdaCommand(OpenReportContractsIsPluralism);

    private ICommand? _openReportJubilee;
    public ICommand OpenReportJubilee => _openReportJubilee ??= new LambdaCommand(OpenReportJubilees);

    #endregion

    private ICommand? _updateStates;
    public ICommand UpdateStates => _updateStates ??= new LambdaCommand(ApiUpdateStatePersonAsync);

    private ICommand? _copyBuffer;
    public ICommand CopyBuffer => _copyBuffer ??= new LambdaCommand(CopyFullNameToBuffer);

    private static void CopyFullNameToBuffer(object obj)
    {
        if( obj is Persons person)
            Clipboard.SetText(person.FullName);

    }

    private ICommand? _openreportCard;
    public ICommand OpenReportCard => _openreportCard ??= new LambdaCommand(ReportPersonCard , _ => SelectedPerson != null);
    
    private ICommand? _openPreview;
    public ICommand OpenPreview => _openPreview ??= new LambdaCommand(OpenPreviewModel);

    private static void OpenPreviewModel(object obj)
    {
        string urlHost = ConfigurationManager.AppSettings["host"]
        ?? throw new NullReferenceException("Uninitialized property: " + nameof(urlHost));

        if (obj is Documents d) new PreviewDocument(urlHost + d.Url).ShowDialog();
    }

    private ICommand? _openAddPosition;
    public ICommand OpenAddPosition => _openAddPosition ??= new LambdaCommand(AddPosition , _ => SelectedPerson != null);

    private ICommand? _openChangePosition;
    public ICommand OpenChangePosition => _openChangePosition ??= new LambdaCommand(ChangePosition, _ => SelectedPerson != null && SelectedPosition != null);

    private ICommand? _openChangeDatePosition;
    public ICommand OpenChangeDatePosition => _openChangeDatePosition ??= new LambdaCommand(ChangeDatePosition, _ => SelectedPerson != null && SelectedPosition != null);

    private ICommand? _openDropPosition;
    public ICommand OpenDropPosition => _openDropPosition ??= new LambdaCommand(DeletePosition, _ => SelectedPerson != null && SelectedPosition != null && SelectedPosition.IsMain != true && SelectedPosition.IsPluralismOter != true);

    private ICommand? _openChangeSurname;
    public ICommand OpenChangeSurname => _openChangeSurname ??= new LambdaCommand(ChangeSurname , _ => SelectedPerson != null);

    private ICommand? _openreportSpravka;
    public ICommand OpenreportSpravka => _openreportSpravka ??= new LambdaCommand(ReportSpravkaCard, _ => SelectedPosition != null);

    private ICommand? _openreportObjective;
    public ICommand OpenreportObjective => _openreportObjective ??= new LambdaCommand(ReportEmploymentHistory, _ => SelectedPerson != null);

    private ICommand? _openExperience;
    public ICommand OpenExperience => _openExperience ??= new LambdaCommand(OpenEmployeeExperience, _ => SelectedPerson != null);

    private ICommand? _loadedListPerson;
    public ICommand LoadedListPerson => _loadedListPerson ??= new LambdaCommand(ApiGetListPersons);

    private ICommand? _loadedListAll;
    public ICommand LoadedListAll => _loadedListAll ??= new LambdaCommand(ApiGetListAllPersonsAsync);

    private ICommand? _uploadPhoto;
    public ICommand UploadPhoto => _uploadPhoto ??= new LambdaCommand(UploadFormPhoto);

    private ICommand? _saveMainInfo;
    public ICommand SaveMainInfo => _saveMainInfo ??= new LambdaCommand(SaveMainInfoByPerson);

    private ICommand? _getBack;
    public ICommand GetBack => _getBack ??= new LambdaCommand(GetBackViewAsync);

    private ICommand? _getInfo;
    public ICommand GetInfo => _getInfo ??= new LambdaCommand(ApiGetInformationToPerson, CanCommandExecute);

    private ICommand? _getPersonsToNppAll;
    public ICommand GetPersonsToNppAll => _getPersonsToNppAll ??= new LambdaCommand(GetNppPersonsAll);

    private ICommand? _getPersonsToNpp;
    public ICommand GetPersonsToNpp => _getPersonsToNpp ??= new LambdaCommand(GetNppPersons);

    private ICommand? _getPersonsToNotNpp;
    public ICommand GetPersonsToNotNpp => _getPersonsToNotNpp ??= new LambdaCommand(GetNoNppPersons);
    //------------------- Работа ------------------------------//
    // Отпуск
    private ICommand? _addVacation;
    public ICommand AddVacation => _addVacation ??= new LambdaCommand(AddVacationsPerson);

    private ICommand? _saveVacation;
    public ICommand SaveVacation => _saveVacation ??= new LambdaCommand(SaveVacationsPerson, _ => 
    SelectedVacation != null
    && SelectedVacation.Type.Length > 0 
    && SelectedVacation.Period.Length > 0);

    private ICommand? _deleteVacation;
    public ICommand DeleteVacation => _deleteVacation ??= new LambdaCommand(DeleteVacationsAsync, _ => SelectedVacation != null);
    
    //------------------- Паспорт ------------------------------//
    
    // Сканы документов
    private ICommand? _addDocument;
    public ICommand AddDocument => _addDocument ??= new LambdaCommand(AddDocumentPerson);

    private ICommand? _saveDocument;
    public ICommand SaveDocument => _saveDocument ??= new LambdaCommand(SaveDocumentPerson, _ => SelectedDocument != null);
    
    private ICommand? _saveUploadDocument;
    public ICommand SaveUploadDocument => _saveUploadDocument ??= new LambdaCommand(UploadDocumentByPerson, _ => SelectedDocument != null);

    private ICommand? _deleteDocument;
    public ICommand DeleteDocument => _deleteDocument ??= new LambdaCommand(DeleteDocumentByPerson, _ => SelectedDocument != null);
    
    // Родственники
    private ICommand? _addFamily;
    public ICommand AddFamily => _addFamily ??= new LambdaCommand(AddFamilyPerson);

    private ICommand? _saveFamily;
    public ICommand SaveFamily => _saveFamily ??= new LambdaCommand(SaveFamilyPerson, _ => _selectedFamily != null);

    private ICommand? _deleteFamily;
    public ICommand DeleteFamily => _deleteFamily ??= new LambdaCommand(DeleteFamilyPerson, _ => SelectedFamily != null);



    private ICommand? _deleteOldSurname;
    public ICommand DeleteOldSurname => _deleteOldSurname ??= new LambdaCommand(AsyncDeleteOldSurname, _ => SelectedOldSurname != null);

    //------------------- ОБРАЗОВАНИЕ ------------------------------//

    // Медицинское образование
    private ICommand? _addMedical;
    public ICommand AddMedical => _addMedical ??= new LambdaCommand(AddEducationMed);

    private ICommand? _saveMedical;
    public ICommand SaveMedical => _saveMedical ??= new LambdaCommand(SaveEducationMed, _ => SelectedMedical != null);

    private ICommand? _deleteMedical;
    public ICommand DeleteMedical => _deleteMedical ??= new LambdaCommand(DeleteEducationMed, _ => SelectedMedical != null);

    // Член-корр.
    private ICommand? _addMember;
    public ICommand AddMember => _addMember ??= new LambdaCommand(AddMemberAcademic);

    private ICommand? _saveMember;
    public ICommand SaveMember => _saveMember ??= new LambdaCommand(SaveMemberAcademic, _ => SeletedMemberAcademic != null);

    private ICommand? _deleteMember;
    public ICommand DeleteMember => _deleteMember ??= new LambdaCommand(DeleteMemberAcademic, _ => SeletedMemberAcademic != null);

    // Награждения
    private ICommand? _addRewarding;
    public ICommand AddRewarding => _addRewarding ??= new LambdaCommand(AddRewardingPerson);

    private ICommand? _saveRewarding;
    public ICommand SaveRewarding => _saveRewarding ??= new LambdaCommand(SaveRewardingPerson, _ => SelectedRewarding != null);

    private ICommand? _deleteRewarding;
    public ICommand DeleteRewarding => _deleteRewarding ??= new LambdaCommand(DeleteRewardingPerson, _ => SelectedRewarding != null);

    // Основное образование
    private ICommand? _addEducation;
    public ICommand AddEducation => _addEducation ??= new LambdaCommand(AddMainEducation);

    private ICommand? _saveEducation;
    public ICommand SaveEducation => _saveEducation ??= new LambdaCommand(SaveMainEducation, _ => SelectedEducation != null);

    private ICommand? _deleteEducation;
    public ICommand DeleteEducation => _deleteEducation ??= new LambdaCommand(DeleteMainEducation, _ => SelectedEducation != null);

    // Повышение квалификации
    private ICommand? _addQualification;
    public ICommand AddQualification => _addQualification ??= new LambdaCommand(AddQualificationEducation);

    private ICommand? _saveQualification;
    public ICommand SaveQualification => _saveQualification ??= new LambdaCommand(SaveQualificationEducation, _ => SelectedQualification != null);

    private ICommand? _deleteQualification;
    public ICommand DeleteQualification => _deleteQualification ??= new LambdaCommand(DeletealificationEducation, _ => SelectedQualification != null);


    // Научная степень
    private ICommand? _addScience;
    public ICommand AddScience => _addScience ??= new LambdaCommand(AddScienceDegree);

    private ICommand? _saveScience;
    public ICommand SaveScience => _saveScience ??= new LambdaCommand(SaveScienceDegree, _ => SeletedDegree != null);

    private ICommand? _deleteScience;
    public ICommand DeleteScience => _deleteScience ??= new LambdaCommand(DeleteScienceDegree, _ => SeletedDegree != null);

    // Ученое звание
    private ICommand? _addTitle;
    public ICommand AddTile => _addTitle ??= new LambdaCommand(AddAcademicTitle);

    private ICommand? _saveTitle;
    public ICommand SaveTile => _saveTitle ??= new LambdaCommand(SaveAcademicTitle, _ => SelectedTitle != null);

    private ICommand? _deleteTitle;
    public ICommand DeleteTitle => _deleteTitle ??= new LambdaCommand(DeleteAcademicTitle, _ => SelectedTitle != null);

    // изменить данные паспорта
    private ICommand? _saveDataPassport;
    public ICommand SaveDataPassport => _saveDataPassport ??= new LambdaCommand(UpdatePassport, _ => SelectedPerson != null);

    // Пенсионеры
    private ICommand? _addPensioner;
    public ICommand AddPensioner => _addPensioner ??= new LambdaCommand(AddPensionerAsync);

    private ICommand? _savePensioner;
    public ICommand SavePensioner => _savePensioner ??= new LambdaCommand(SavePensionerAsync, _ => SelectedPens != null);

    private ICommand? _deletePensioner;
    public ICommand DeletePensioner => _deletePensioner ??= new LambdaCommand(DeletePensionerAsync, _ => SelectedPens != null);

    // Инвалиды
    private ICommand? _addInvalid;
    public ICommand AddInvalid => _addInvalid ??= new LambdaCommand(AddInvalidAsync);

    private ICommand? _saveInvalid;
    public ICommand SaveInvalid => _saveInvalid ??= new LambdaCommand(SaveInvalidAsync, _ => SelectedInvalid != null);

    private ICommand? _deleteInvalid;
    public ICommand DeleteInvalid => _deleteInvalid ??= new LambdaCommand(DeleteInvalidAsync, _ => SelectedInvalid != null);

    // Трудовая книга

    private ICommand? _addHistory;
    public ICommand AddHistory => _addHistory ??= new LambdaCommand(AddHistoryAsync);

    // Изменить трудовую
    private ICommand? _saveHistory;
    public ICommand SaveHistory => _saveHistory ??= new LambdaCommand(SaveHistoryAsync, _ => SelectedHistory != null);

    private ICommand? _deleteHistory;
    public ICommand DeleteHistory => _deleteHistory ??= new LambdaCommand(DeleteHistoryAsync, _ => SelectedHistory != null);

    #endregion


    #region Methods Menu Items
    private async void Exit(object p)
    {
        try
        {
            await QueryService.JsonSerializeWithToken(token: _user!.Token,
                "/logout",
                "POST",
                User);
            // Вернуть на Авторизацию 
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void OpenReportContractsIsPluralism(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        InsertReportViewModel viewModel = new("/reports/pers/persons/contract/is_pluralism/", "Истечении срока действия трудового договора(Совместители)", _user!);
        ReportByPersonInsert view = new() { DataContext = viewModel };
        view.ShowDialog();

    }
    private void OpenReportJubilees(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        ReportViewModelJubilee viewModel = new("/reports/pers/persons/jubilee/", "Список сотрудников юбиляров", _user!);
        RepotViewJubilee view = new() { DataContext = viewModel };
        view.ShowDialog();

    }
    private void OpenTypeVacationView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeVacationViewModel(_navigationStore, _user!);
    }

    private void OpenTypeRewardingView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeRewardingViewModel(_navigationStore, _user!);
    }

    private void OpenPeriodView(object p)
    {
        _navigationStore.CurrentViewModel = new PeriodVacationViewModel(_navigationStore, _user!);
    }
    // Отчеты
    private void OpenReportView(object p)
    {
        _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user!);
    }
    // Отчет принятых
    private void OpenReportInsertPerson(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        InsertReportViewModel viewModel = new("/reports/pers/persons/insert/", "Отчет принятых", _user!);
        ReportByPersonInsert view = new() { DataContext = viewModel };
        view.ShowDialog();

    }

    private void OpenReportInsertPersonIsOther(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        InsertReportViewModel viewModel = new("/reports/pers/persons/insert/is_pluralism/", "Отчет принятых(Совместителей)", _user!);
        ReportByPersonInsert view = new() { DataContext = viewModel };
        view.ShowDialog();

    }
    // Отчет уволенных
    private void OpenReportDropPerson(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        InsertReportViewModel viewModel = new("/reports/pers/persons/drop/", "Отчет уволенных", _user!);
        ReportByPersonInsert view = new() { DataContext = viewModel };
        view.ShowDialog();

    }
    private void OpenReportDropPersonIsOther(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        InsertReportViewModel viewModel = new("/reports/pers/persons/drop/is_pluralism/", "Отчет уволенных", _user!);
        ReportByPersonInsert view = new() { DataContext = viewModel };
        view.ShowDialog();

    }
    // Отчет награжденных
    private void OpenReportRewardingPerson(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        InsertReportViewModel viewModel = new("/reports/pers/persons/rewarding/", "Отчет награжденных", _user!);
        ReportByPersonInsert view = new() { DataContext = viewModel };
        view.ShowDialog();

    }
    // Отчет остатков отпусков по периоду
    private void OpenReportVacationPeriod(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        ReportVacationViewModel viewModel = new("/reports/pers/persons/vacation/residue/", "Список остатков отпусков", _user!);
        ReportVacation view = new() { DataContext = viewModel };
        view.ShowDialog();

    }

    private void OpenReportContracts(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        InsertReportViewModel viewModel = new("/reports/pers/persons/contract/", "Истечении срока действия трудового договора", _user!);
        ReportByPersonInsert view = new() { DataContext = viewModel };
        view.ShowDialog();

    }

    private void OpenEmployeeExperience(object p)
    {
        // _navigationStore.CurrentViewModel = new ReportsViewModel(_navigationStore, _user);
        EmployeeExperienceViewModel viewModel = new(SelectedPerson!);
        EmployeeExperience view = new() { DataContext = viewModel };
        view.ShowDialog();

    }
    // Новый отчет
    private void OpenMasterReportView(object p)
    {
        _navigationStore.CurrentViewModel = new MasterReportViewModel(_navigationStore, _user!);
    }

    private void OpenMasterDropView(object p)
    {
        _navigationStore.CurrentViewModel = new MasterDropViewModel(_navigationStore, _user!);
    }
    // Отделы
    private void OpenDepartmentView(object p)
    {
        _navigationStore.CurrentViewModel = new DepartmentViewModel(_navigationStore, _user!);
    }
    // Типы приказов
    private void OpenTypeOrderView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeOrderViewModel(_navigationStore, _user!);
    }
    // Тип Званий
    private void OpenTypeRankView(object p)
    {
        _navigationStore.CurrentViewModel = new TypeRanksViewModel(_navigationStore, _user!);
    }
    // Приказы
    private void OpenOrderView(object p)
    {
        _navigationStore.CurrentViewModel = new OrderViewModel(_navigationStore, _user!);
    }

    private void OpenPositionView(object p)
    {
        _navigationStore.CurrentViewModel = new PositionViewModel(_navigationStore, _user!);
    }
    // Поиск
    private void OpenSearchView(object p)
    {
        _navigationStore.CurrentViewModel = new SearchViewModel(_navigationStore, _user!);
    }

    private void OpenTypePositionView(object p)
    {
        _navigationStore.CurrentViewModel = new TypePositionViewModel(_navigationStore, _user!);
    }

    #endregion

    #region Основная Логика

    // Распечатать Справку с места работы
    private async void ReportSpravkaCard(object p)
    {
        try
        {
            IsLoading = true;

            object payload = new
            {
                id_person = _selectedPerson!.Id,
                id_person_position = _selectedPosition!.Id,

            };
            await ReportService.JsonPostWithToken(
                    payload,
                    token: _user!.Token,
                    queryUrl: "/reports/pers/persons/spravka/",
                    httpMethod: "POST",
                    reportName: "Справка с места роботы");

            IsLoading = false;

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    // Объективка
    private async void ReportEmploymentHistory(object p)
    {
        try
        {
            IsLoading = true;

            await ReportService.JsonPostWithToken(
                    SelectedPerson!,
                    token: _user!.Token,
                    queryUrl: "/reports/pers/persons/objective/",
                    httpMethod: "POST",
                    reportName: "Объективка");

            IsLoading = false;

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


    private void AddPosition(object p)
    {
        AddPositionViewModel viewModel = new(_user! , SelectedPerson!);
        AddPositionView view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetInformationToPerson(p);
    }

    private void ChangePosition(object p)
    {
        ChangePositionViewModel viewModel = new(_user!, SelectedPerson! , SelectedPosition!);
        ChangePosition view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetInformationToPerson(p);
    }

    private void ChangeDatePosition(object p)
    {
        ChangeDateContractViewModel viewModel = new(_user!, SelectedPerson!, SelectedPosition!);
        ChangeDateContractView view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetInformationToPerson(p);
    }
    private void DeletePosition(object p)
    {
        DeletePositionViewModel viewModel = new($"С должности: '{SelectedPosition!.Name}'", _user!, SelectedPerson! , SelectedPosition!);
        DeletePositionView view = new() { DataContext = viewModel };
        view.ShowDialog();
        // Обновить данные
        ApiGetInformationToPerson(p);
    }
    // Распечатать личную карту
    private async void ReportPersonCard(object p)
    {
        try
        {
            IsLoading = true;

            await ReportService.JsonDeserializeWithToken(
                    token: _user!.Token,
                    queryUrl: "/reports/pers/persons/card/" + _selectedPerson!.Id,
                    httpMethod: "GET",
                    reportName: "Личная карта");

            IsLoading = false;

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    // Api Информация персоны
    private async void ApiGetInformationToPerson(object p)
    {
        try
        {
            IsLoading = true;
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");

            //Служебные перемещения 
            if(SelectedPerson!.ArrayMove?.Count > 0)
            {
                SelectedMove = SelectedPerson.ArrayMove[0];
            }
            //Отпуска 
            if (SelectedPerson!.ArrayVacation?.Count > 0)
            {
                SelectedVacation = SelectedPerson.ArrayVacation[0];
            }
            // Образование
            if (SelectedPerson!.ArrayEducation?.Count > 0)
            {
                SelectedEducation = SelectedPerson.ArrayEducation[0];
            }
            // Члены/корре.
            if (SelectedPerson!.ArrayMeberAcademic?.Count > 0)
            {
                SeletedMemberAcademic = SelectedPerson.ArrayMeberAcademic[0];
            }

            if (SelectedPerson!.ArrayAcademicTitle?.Count > 0)
            {
                SelectedTitle = SelectedPerson.ArrayAcademicTitle[0];
            }
            // Повышение квалификации
            if (SelectedPerson!.ArrayQualification?.Count > 0)
            {
                SelectedQualification = SelectedPerson.ArrayQualification[0];
            }
            // Научные степени
            if (SelectedPerson!.ArrayScientificDegree?.Count > 0)
            {
                SeletedDegree = SelectedPerson.ArrayScientificDegree[0];
            }

            // Документы
            if (SelectedPerson!.ArrayDocuments?.Count > 0)
            {
                SelectedDocument = SelectedPerson.ArrayDocuments[0];
            }

            // Награждения
            if (SelectedPerson!.ArrayRewarding?.Count > 0)
            {
                SelectedRewarding = SelectedPerson.ArrayRewarding[0];
            }

            // После получение информации рассчитать стаж
            if (SelectedPerson!.HistoryEmployment?.Count > 0)
            {
                // Трудовая
                SelectedHistory = SelectedPerson.HistoryEmployment[0];
                StageIsOver = ServiceWorkingExperience.GetStageIsOver(SelectedPerson.HistoryEmployment , DateTime.Now);
                StageIsUniver = ServiceWorkingExperience.GetStageIsUniver(SelectedPerson.HistoryEmployment, DateTime.Now);
                StageIsScience = ServiceWorkingExperience.GetStageIsScience(SelectedPerson.HistoryEmployment, DateTime.Now);
                StageIsPedagogical = ServiceWorkingExperience.NewGetStageIsPedagogical(SelectedPerson.HistoryEmployment , DateTime.Now);
                StageIsMedical = ServiceWorkingExperience.GetStageIsMedical(SelectedPerson.HistoryEmployment, DateTime.Now);
                StageIsMuseum = ServiceWorkingExperience.GetStageIsMuseum(SelectedPerson.HistoryEmployment, DateTime.Now);
                StageIsLibrary = ServiceWorkingExperience.GetStageIsLibrary(SelectedPerson.HistoryEmployment, DateTime.Now);

            }

            IsLoading = false;
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    // Преобразование Фото в base64
    private static string GetBase64FromImage(string path)
    {
        using var image = Image.FromFile(path);
        using MemoryStream m = new();
        image.Save(m, image.RawFormat);
        var imageBytes = m.ToArray();

        // Convert byte[] to Base64 String
        var base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }

    // Вернуться на главную страницу
    private async void GetBackViewAsync(object p)
    {
        _department = await QueryService.JsonDeserializeWithObject<Departments>(token: _user!.Token,
                                                                                "/pers/tree/find/" + _idDepartment,
                                                                                "GET");
        _navigationStore.CurrentViewModel = new HomeViewModel(_user!, _navigationStore, _department!);
    }


    private void GetNppPersons(object p)
    {
        if (CollectionPerson == null) return;
        CollectionPerson.Filter = FilterIsNpp;
        CollectionPerson.Refresh();
    }

    private void GetNoNppPersons(object p)
    {
        if (CollectionPerson == null) return;
        CollectionPerson.Filter = FilterIsNoNpp;
        CollectionPerson.Refresh();
    }
    private void GetNppPersonsAll(object p)
    {
        if (CollectionPerson == null) return;
        CollectionPerson.Filter = null;
        CollectionPerson.Refresh();
    }

    private async void ChangeSurname(object p)
    {
        ChangeSurnameViewModel viewModel = new(_user!, SelectedPerson!);
        ChangeSurnameView view = new() { DataContext = viewModel };
        view.ShowDialog();

        SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");

        // var change = PersonsList?.FirstOrDefault(x => x.Id == SelectedPerson?.Id).FirstName;
        ObservableCollection<Persons> newArray = new();

        if (PersonsList != null)
            foreach (var item in PersonsList)
            {
                if (item.Id == SelectedPerson?.Id)
                {
                    item.FirstName = SelectedPerson.FirstName;
                }

                newArray.Add(item);
            }

        PersonsList = newArray;


    }

    private async void SaveMainInfoByPerson(object p)
    {
        try
        {
            IsLoading = true;
            if (SelectedPerson == null) return;

            object person = new
            {
                id = SelectedPerson.Id,
                name = SelectedPerson.MidlleName,
                lastname = SelectedPerson.LastName,
                gender = RadioIsMale ? "male" : "female",
                birthday = SelectedPerson.Birthday,
                phone_ua = SelectedPerson.PhoneUkraine,
                phone_lug = SelectedPerson.PhoneLugakom,
                date_to_working = SelectedPerson.DateWorking,
                description = SelectedPerson.Description,
                position_pluralist = SelectedPerson.PositionPluralist,
            };

            await QueryService.JsonSerializeWithToken(token: _user!.Token,
                "/pers/person/save",
                "POST"
                ,person);

            ApiGetInformationToPerson(p);

            //ApiGetListAllPersonsAsync(p);
            _ = MessageBox.Show("Данные обновлены!", "Сообщение");
            IsLoading = false;

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            IsLoading = false;
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            IsLoading = false;

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Загрузка изображений на человека
    private async void UploadFormPhoto(object p)
    {
        try
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                DefaultExt = ".jpg", // Default file extension
                Filter = "Image files(*.jpg, *.jpeg) | *.jpg; *.jpeg;"
            };
            if (openFileDialog.ShowDialog() != true) return;
            var base64 = GetBase64FromImage(openFileDialog.FileName);
            IsLoading = true;
            // Отправляем base64 image 
            await QueryService.JsonSerializeWithToken(token: _user!.Token,
                "/pers/person/upload/" + SelectedPerson!.Id,
                "PUT",
                new { photo = base64 });

            _ = MessageBox.Show("Фотография загружена");
            ApiGetInformationToPerson(p);
            //SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(_user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");

            /*Avatar.BeginInit();
                Avatar.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                Avatar.CacheOption = BitmapCacheOption.OnLoad;
                Avatar.UriSource = new Uri(SelectedPerson.Photo);
                Avatar.EndInit();
                */
            IsLoading = false;
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void ApiUpdateStatePersonAsync(object obj)
    {
        try
        {
            IsLoading = true;

            // Изменить статус персону
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/update/status", "POST", SelectedPerson);

            IsLoading = false;


        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    // Полный список сотрдуников
    private async void ApiGetListAllPersonsAsync(object obj)
    {
        try
        {
            IsLoading = true;
            // Получить список людей всех отделов
            var array = await QueryService.JsonDeserializeWithToken<Persons>(token: _user!.Token, "/pers/person/get/all", "GET");
            // TODO: Если что-то сломается (Убери!)
            PersonsList = array.DistinctBy(x => x.FullName);
            IsLoading = false;


        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    // Вернуть список людей из отдела
    private async void ApiGetListPersons(object p)
    {
        try
        {
            var currentYear = DateTime.Today.Year;
            var prevYear = DateTime.Today.AddYears(-2);

            IsLoading = true;
            // Справочник отделов
            //Departments = await QueryService.JsonDeserializeWithToken<Departments>(_user!.Token, "/pers/tree/all", "GET");
            // Справочник должностей
            //TypePositions = await QueryService.JsonDeserializeWithToken<TypePosition>(_user!.Token, "/pers/position/type/position", "GET");
            // Справочник Мест работы
            //TypePlaceOfWorks = await QueryService.JsonDeserializeWithToken<PlaceOfWork>(_user!.Token, "/pers/position/type/place", "GET");
            // Справочник контрактов
            //TypeContracts = await QueryService.JsonDeserializeWithToken<TypeContract>(token: _user!.Token, "/pers/position/type/contract", "GET");
            // Справочник паспортов
            TypePassports = await QueryService.JsonDeserializeWithToken<TypePassport>(token: _user!.Token, "/pers/person/get/passport", "GET");
            // Справочник (Период отпусков)
            PeriodVacation = await QueryService.JsonDeserializeWithToken<PeriodVacation>(token: _user!.Token, "/pers/vacation/period/get", "GET");
            // Справочние (Тип отпуска)
            TypeVacation = await QueryService.JsonDeserializeWithToken<Models.TypeVacation>(token: _user!.Token, "/pers/vacation/type/get", "GET");
            // Справочник тип родства
            TypeFamily = await QueryService.JsonDeserializeWithToken<TypeFamily>(token: _user!.Token, "/pers/person/family/type", "GET");
            // Вид пенсионера
            TypePensioner = await QueryService.JsonDeserializeWithToken<TypePensioner>(token: _user!.Token, "/pers/person/pensioner/type", "GET");
            // Тип образования 
            TypeEducation = await QueryService.JsonDeserializeWithToken<TypeEducation>(token: _user!.Token, "/pers/education/type/get", "GET");
            // Тип награждения
            TypeRewarding = await QueryService.JsonDeserializeWithToken<Rewarding>(token: _user!.Token, "/pers/rewarding/type/get", "GET");
            // Приказы для награждения
            var orderRewarding = await QueryService.JsonDeserializeWithToken<Rewarding>(token: _user!.Token, "/pers/order/get/9", "GET");
            OrderRewarding = orderRewarding.Where(x => x.DateOrder.Date.Year == currentYear || x.DateOrder.Date.Year == prevYear.Year);

            // Приказы для отпусков
            var orderVacation = await QueryService.JsonDeserializeWithToken<Vacation>(token: _user!.Token, "/pers/order/get/1", "GET");
            OrderVacations = orderVacation.Where(x => x.DateOrder.Date.Year == currentYear || x.DateOrder.Date.Year == prevYear.Year);

            // Приказы для смены фамилии
            var orderSurname = await QueryService.JsonDeserializeWithToken<Order>(token: _user!.Token, "/pers/order/get/5", "GET");
            OrderOldSurname = orderSurname.Where(x => x.DateOrder.Date.Year == currentYear || x.DateOrder.Date.Year == prevYear.Year);

            // Мед.категория 
            MedicalCategory = await QueryService.JsonDeserializeWithToken<MedicalCategory>(token: _user!.Token, "/pers/medical/type/get", "GET");
            // Ученая степень
            TypeDegree = await QueryService.JsonDeserializeWithToken<TypeDegree>(token: _user!.Token, "/pers/scientific/type/get", "GET");
            // Член.корр.
            TypeRanks = await QueryService.JsonDeserializeWithToken<TypeRank>(token: _user!.Token, "/pers/member/type/get", "GET");
            // Тип документов (сканов)
            TypeDocuments = await QueryService.JsonDeserializeWithToken<TypeDocuments>(token: _user!.Token, "/pers/document/type/get", "GET");
            // Тут оставляем всё как есть
            TypeTitle = TypeRanks;
            // Показываем только два элемента для Членов акдемиков
            TypeRanks = TypeRanks.Where(x => x.Name is "Академик" or "Член-корреспондент");
            // Получить список людей в отделе
            PersonsList = await QueryService.JsonDeserializeWithToken<Persons>(token: _user!.Token, "/pers/person/get/short/" + _idDepartment, "GET");
            // PersonsList = await _Api.GetListPersonsToDepartment(_User!.Token, _Department!.Id);
            // Маленький костыль, для того чтобы находить на каком item я должен стоять при загрузке личной карты
            var indeArray = PersonsList.Select((value, index) => (Value: value, Index: index))
                .FirstOrDefault(valueTuple => valueTuple.Value.Id == SelectedPerson!.Id);
            // Выбрать текущего человека при загрузке Window
            SelectedPerson = indeArray.Value;

            IsLoading = false;
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private bool CanCommandExecute(object p) => _selectedPerson is not null;
    private bool FilterToPerson(object emp) => string.IsNullOrEmpty(FilterPerson) || emp is Persons pers && pers.FullName.ToUpper().Contains(FilterPerson.ToUpper());
    private static bool FilterIsNpp(object emp) => emp is Persons { IsPed: true };
    private static bool FilterIsNoNpp(object emp) => emp is Persons { IsPed: false };

    #endregion

    #region Образование

    /* Основное образование */
    private async void AddMainEducation(object p)
    {
        var count = _selectedPerson!.ArrayEducation?.Where(x => x.Id == 0).ToList().Count;
        if (count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        try
        {
            Education order = new()
            {
                TypeName = "Бакалавр",
                IsActual = true,
                DateIssue = DateTime.Now,
                Institution = "",

            };
            _selectedPerson!.ArrayEducation?.Insert(0, order);
            SelectedEducation = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveMainEducation(object p)
    {
        try
        {
            SelectedEducation!.IdPerson = SelectedPerson!.Id;

            if (SelectedEducation!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/education/rename/", "POST", SelectedEducation);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/education/add", "POST", SelectedEducation);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    private async void DeleteMainEducation(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/education/del/" + SelectedEducation!.Id, "DELETE", SelectedEducation);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayEducation!.Remove(SelectedEducation);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /* Повышение квалификации */
    private async void AddQualificationEducation(object p)
    {
        try
        {
            var count = _selectedPerson!.ArrayQualification?.Where(x => x.Id == 0).ToList().Count;
            if (count > 0)
            {
                _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Qualification order = new()
            {
                NameCourse = "Курс",
                Certificate = "№ 0000",
                DateBegin = DateTime.Now,
                DateEnd = DateTime.Now,
                DateIssue = DateTime.Now,
                Place = "Место выдачи"

            };
            _selectedPerson!.ArrayQualification?.Insert(0, order);
            SelectedQualification = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveQualificationEducation(object p)
    {
        try
        {
            SelectedQualification!.IdPerson = SelectedPerson!.Id;

            if (SelectedQualification!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/qualification/rename/", "POST", SelectedQualification);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/qualification/add", "POST", SelectedQualification);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    private async void DeletealificationEducation(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/qualification/del/" + SelectedQualification!.Id, "DELETE", SelectedQualification);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayQualification!.Remove(SelectedQualification);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /* Медицинское образование */
    private async void AddEducationMed(object p)
    {
        try
        {
            var count = _selectedPerson!.ArrayMedical?.Where(x => x.Id == 0).ToList().Count;
            if (count > 0)
            {
                _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Medical order = new()
            {
                Category = "Аттестация",
                Name = "Новый элемент",
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
            };
            _selectedPerson!.ArrayMedical?.Insert(0, order);
            SelectedMedical = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveEducationMed(object p)
    {
        try
        {
            SelectedMedical!.IdPerson = SelectedPerson!.Id;

            if (SelectedMedical!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/medical/rename/", "POST", SelectedMedical);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/medical/add", "POST", SelectedMedical);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    private async void DeleteEducationMed(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/medical/del/" + SelectedMedical!.Id, "DELETE", SelectedMedical);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayMedical!.Remove(SelectedMedical);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /* Член-корр. */
    private async void AddMemberAcademic(object p)
    {
        try
        {
            var count = _selectedPerson!.ArrayMeberAcademic?.Where(x => x.Id == 0).ToList().Count;
            if (count > 0)
            {
                _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MemberAcademic order = new()
            {
                Document = "№ 0000",
                NameAcademic = "",
                Datebegin = DateTime.Now,
                Name = "Академик",

            };
            _selectedPerson!.ArrayMeberAcademic?.Insert(0, order);
            SeletedMemberAcademic = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveMemberAcademic(object p)
    {
        try
        {
            SeletedMemberAcademic!.IdPerson = SelectedPerson!.Id;

            if (SeletedMemberAcademic!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/member/rename/", "POST", SeletedMemberAcademic);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/member/add", "POST", SeletedMemberAcademic);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    private async void DeleteMemberAcademic(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/member/del/" + SeletedMemberAcademic!.IdMember, "DELETE", SeletedMemberAcademic);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayMeberAcademic!.Remove(SeletedMemberAcademic);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /* Награждения */
    private async void AddRewardingPerson(object p)
    {
        try
        {
            var count = _selectedPerson!.ArrayRewarding?.Where(x => x.Id == 0).ToList().Count;
            if (count > 0)
            {
                _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Rewarding order = new()
            {
                NumberDocumet = "",
                Type = "Благодарность",
                DateIssue = DateTime.Now,


            };
            _selectedPerson!.ArrayRewarding?.Insert(0, order);
            SelectedRewarding = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveRewardingPerson(object p)
    {
        try
        {
            SelectedRewarding!.IdPerson = SelectedPerson!.Id;

            //SelectedRewarding!.IdOrder = 

            if (SelectedRewarding!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/rewarding/rename/", "POST", SelectedRewarding);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/rewarding/add", "POST", SelectedRewarding);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    private async void DeleteRewardingPerson(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/rewarding/del/" + SelectedRewarding!.Id, "DELETE", SelectedRewarding);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayRewarding!.Remove(SelectedRewarding);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void AddScienceDegree(object p)
    {
        try
        {
            var count = _selectedPerson!.ArrayScientificDegree?.Where(x => x.Id == 0).ToList().Count;
            if (count > 0)
            {
                _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ScientificDegree order = new()
            {
               DateOfIssue = DateTime.Now,
               Document = "№ 0000",
               City = "Город",
               Place = "Место",
               ScientificBranch = "Отрасль",
               ScientificSpecialty = "Специальность"

            };
            _selectedPerson!.ArrayScientificDegree?.Insert(0, order);
            SeletedDegree = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /* Ученое звание */
    private async void AddAcademicTitle(object p)
    {
        try
        {
            var count = _selectedPerson!.ArrayAcademicTitle?.Where(x => x.Id == 0).ToList().Count;
            if (count > 0)
            {
                _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            AcademicTitle order = new()
            {
                Department = "Наименование подразделения",
                Document = "№ 0000",
                Place = "Место присвоения",
                DateIssue = DateTime.Now,

            };
            _selectedPerson!.ArrayAcademicTitle?.Insert(0, order);
            SelectedTitle = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveScienceDegree(object p)
    {
        try
        {
            SeletedDegree!.IdPerson = SelectedPerson!.Id;

            //SelectedRewarding!.IdOrder = 

            if (SeletedDegree!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/scientific/rename", "POST", SeletedDegree);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/scientific/add", "POST", SeletedDegree);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    private async void SaveAcademicTitle(object p)
    {
        try
        {
            SelectedTitle!.IdPerson = SelectedPerson!.Id;

            //SelectedRewarding!.IdOrder = 

            if (SelectedTitle!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/rank/rename/", "POST", SelectedTitle);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/rank/add", "POST", SelectedTitle);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    private async void DeleteScienceDegree(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/scientific/del/" + SeletedDegree!.Id, "DELETE", SeletedDegree);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayScientificDegree!.Remove(SeletedDegree);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void DeleteAcademicTitle(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/rank/del/" + SelectedTitle!.Id, "DELETE", SelectedTitle);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayAcademicTitle!.Remove(SelectedTitle);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


    #endregion

    #region Паспорт данные

    // Сканы документов

    // Сохранить изменения паспорта
    private async void UpdatePassport(object p)
    {
        try
        {
            if (SelectedPerson!.SerialPassport.Length is 0 or > 12)
            {
                _ = MessageBox.Show("Не допустимый размер поля 'Серия паспорта'");
                return;
            }

            if (SelectedPerson!.NumberPassport.Length is 0 or > 12)
            {
                _ = MessageBox.Show("Не допустимый размер поля 'Номер паспорта'");
                return;
            }

            if (SelectedPerson!.Id > 0)
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/rename/passport", "POST", SelectedPerson);

            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    // Пенсионер
    private void AddPensionerAsync(object p)
    {
        var count = _selectedPerson!.ArrayPensioner?.Where(x => x.Id == 0).ToList().Count;
        if (count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        Pensioner order = new()
        {
            DateDocument = DateTime.Now,
            Document = "№ 0000",

        };
        _selectedPerson!.ArrayPensioner?.Insert(0, order);
        SelectedPens = order;

    }
    private async void SavePensionerAsync(object p)
    {
        try
        {
            SelectedPens!.IdPerson = SelectedPerson!.Id;

            if (SelectedPens!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/pensioner/rename", "POST", SelectedPens);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/pensioner/add", "POST", SelectedPens);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void DeletePensionerAsync(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/pensioner/del/" + SelectedPens!.Id, "DELETE", SelectedPens);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayPensioner!.Remove(SelectedPens);
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    // Инвалидность
    private  void AddInvalidAsync(object p)
    {
        var count = _selectedPerson!.ArrayInvalid?.Where(x => x.Id == 0).ToList().Count;
        if (count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        Invalid order = new()
        {
            DateStart = DateTime.Now,
            Document = "№ 0000",
            Group = "1"


        };
        _selectedPerson!.ArrayInvalid?.Insert(0, order);
        SelectedInvalid = order;

    }
    private async void SaveInvalidAsync(object p)
    {
        try
        {
            SelectedInvalid!.IdPerson = SelectedPerson!.Id;

            if (SelectedInvalid!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/invalid/rename", "POST", SelectedInvalid);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/invalid/add", "POST", SelectedInvalid);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void DeleteInvalidAsync(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/invalid/" + SelectedInvalid!.Id, "DELETE", SelectedInvalid);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayInvalid!.Remove(SelectedInvalid);
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void DeleteHistoryAsync(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/history/" + SelectedHistory!.Id, "DELETE", SelectedHistory);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.HistoryEmployment!.Remove(SelectedHistory);
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    // Отпуск
    private void AddVacationsPerson(object p)
    {
        var count = _selectedPerson!.ArrayVacation?.Where(x => x.Id == 0).ToList().Count;
        if (count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        Vacation order = new()
            {
                DateBegin = DateTime.Now,
                DateEnd = DateTime.Now,
                LengthVacation = 0,
                Residue = 0,
                Type = ""

            };
            _selectedPerson!.ArrayVacation?.Insert(0, order);
            SelectedVacation = order;
    }
    private async void DeleteVacationsAsync(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отпуска?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/vacation/del/" + SelectedVacation!.Id, "DELETE", SelectedVacation);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayVacation!.Remove(SelectedVacation);
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    // Документ
    private void AddDocumentPerson(object p)
    {
        var count = _selectedPerson!.ArrayDocuments?.Where(x => x.Id == 0).ToList().Count;
        if (count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        Documents order = new()
        {
            Name = "Наименование документа",
            Type = "Копия паспорта",
            Url = string.Empty,

        };
        _selectedPerson!.ArrayDocuments?.Insert(0, order);
        SelectedDocument = order;
    }
    // Статус семьи
    private  void AddFamilyPerson(object p)
    {
        var count = _selectedPerson!.ArrayFamily?.Where(x => x.Id == 0).ToList().Count;
        if (count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        Family order = new()
        {
            FullName = "ФИО",
            Birthday = DateTime.Now,
            Description = "Примечание",

        };
        _selectedPerson!.ArrayFamily?.Insert(0, order);
        SelectedFamily = order;
    }
    private async void SaveVacationsPerson(object p)
    {
        try
        {
            if(SelectedVacation!.LengthVacation <= 0)
            {
                _ = MessageBox.Show("Длина отпуска равна меньше или равна 0", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (SelectedVacation!.DateBegin == null)
            {
                _ = MessageBox.Show("Дата не указана!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SelectedVacation!.IdPerson = SelectedPerson!.Id;
            //SelectedVacation!.Residue += SelectedVacation.LengthVacation;
            SelectedVacation!.DateEnd = SelectedVacation.DateBegin.Value.AddDays(SelectedVacation.LengthVacation);

            if (SelectedVacation!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/vacation/update", "POST", SelectedVacation);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/vacation/add", "POST", SelectedVacation);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    private void UploadDocumentByPerson(object p)
    {
        
        Microsoft.Win32.OpenFileDialog openFileDialog = new()
        {
            DefaultExt = ".jpg", // Default file extension
            Filter = "Image files(*.jpg, *.jpeg) | *.jpg; *.jpeg;"
        };
        if (openFileDialog.ShowDialog() != true) return;
        var base64 = GetBase64FromImage(openFileDialog.FileName);

        UrlDocument = openFileDialog.FileName;
        SelectedDocument!.Url = UrlDocument;
        SelectedDocument!.Base64 = base64;
    }

    private async void SaveDocumentPerson(object p)
    {
        try
        {
            var newSelectedDocument = SelectedDocument!;
            newSelectedDocument.IdPerson = SelectedPerson!.Id;
  
            if (SelectedDocument!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/document/rename", "POST", newSelectedDocument);
            }
            else
            {
                if (SelectedDocument!.Base64.Length == 0)
                {
                    _ = MessageBox.Show("Необходимо выбрать изображение скана!", "Ошибка", MessageBoxButton.OK , MessageBoxImage.Error);
                    return;
                }
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/document/add", "POST", newSelectedDocument);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            if (SelectedPerson.ArrayDocuments != null)
                SelectedDocument =
                    SelectedPerson.ArrayDocuments.FirstOrDefault(x => x.Name == newSelectedDocument.Name);
            _ = MessageBox.Show("Данные успешно сохраненны");
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    private async void DeleteDocumentByPerson(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данную запись?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/document/del/" + SelectedDocument!.Id, "DELETE", SelectedDocument);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayDocuments!.Remove(SelectedDocument);
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveFamilyPerson(object p)
    {
        try
        {
            SelectedFamily!.IdPerson = SelectedPerson!.Id;

            if (SelectedFamily!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/family/rename", "POST", SelectedFamily);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/family/add", "POST", SelectedFamily);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    private async void AsyncDeleteOldSurname(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/surname/del/" + SelectedOldSurname!.Id, "DELETE", SelectedOldSurname);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayChangeSurname!.Remove(SelectedOldSurname);
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void DeleteFamilyPerson(object p)
    {
        try
        {
            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/family/" + SelectedFamily!.Id, "DELETE", SelectedFamily);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _selectedPerson!.ArrayFamily!.Remove(SelectedFamily);
        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


    #endregion

    #region Работа

    // Трудовая книга
    private  void AddHistoryAsync(object p)
    {
        var count = _selectedPerson!.HistoryEmployment?.Where(x => x.Id == 0).ToList().Count;
        if (count > 0)
        {
            _ = MessageBox.Show("Вы не сохранили предыдущую запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        HistoryEmployment order = new()
        {
            CreateAt = DateTime.Now,
            Name = "Наименование",
            Record = 0

        };
        _selectedPerson!.HistoryEmployment?.Insert(0, order);
        SelectedHistory = order;

    }

    private async void SaveHistoryAsync(object p)
    {
        try
        {
            SelectedHistory!.IdPerson = SelectedPerson!.Id;

            if (SelectedHistory!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/history/rename/" + SelectedHistory.Id, "POST", SelectedHistory);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user!.Token, "/pers/person/history/add", "POST", SelectedHistory);
            }
            // Информация о сотруднике
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
            _ = MessageBox.Show("Данные успешно сохраненны");

        }
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            _ = MessageBox.Show("Скорее всего время токена истекло! ", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

    #endregion
}

