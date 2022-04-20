
using AlphaPersonel.Models.Home;
using AlphaPersonel.Views.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AlphaPersonel.ViewModels;

internal class PersonCardViewModel : BaseViewModel
{
    public PersonCardViewModel(Users users, NavigationStore navigationStore, Persons person, int idDepartment)
    {
        _navigationStore = navigationStore;
        _SelectedPerson = person;
        _idDepartment = idDepartment;
        _user = users;
    }

    #region Стаж

    // Стаж общий
    private string _stageIsOver;
    public string StageIsOver
    {
        get => _stageIsOver;
        private set => Set(ref _stageIsOver, value);
    }
    // Стаж в Универе
    private string _stageIsUniver;
    public string StageIsUniver
    {
        get => _stageIsUniver;
        private set => Set(ref _stageIsUniver, value);
    }

    // Стаж Научный
    private string _stageIsScience;
    public string StageIsScience
    {
        get => _stageIsScience;
        private set => Set(ref _stageIsScience, value);
    }

    // Стаж Научно-Педагогический
    private string _stageIsPedagogical;
    public string StageIsPedagogical
    {
        get => _stageIsPedagogical;
        set => Set(ref _stageIsPedagogical, value);
    }

    // Стаж Медицинский
    private string _stageIsMedical;
    public string StageIsMedical
    {
        get => _stageIsMedical;
        private set => Set(ref _stageIsMedical, value);
    }

    // Стаж Музея
    private string _stageIsMuseum;
    public string StageIsMuseum
    {
        get => _stageIsMuseum;
        private set => Set(ref _stageIsMuseum, value);
    }

    // Стаж Библиотека
    private string _stageIsLibrary;
    public string StageIsLibrary
    {
        get => _stageIsLibrary;
        set => Set(ref _stageIsLibrary, value);
    }

    #endregion

    #region Переменные

    private readonly int _idDepartment = 0;

    private readonly NavigationStore _navigationStore;


   /* private BitmapImage? _Avatar = new();
    public BitmapImage? Avatar
    {
        get => _Avatar;
        set => Set(ref _Avatar, value);

    }
   */

    private Users? _user;
    public Users? User
    {
        get => _user;
        set => Set(ref _user, value);

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

            if (CollectionPerson != null) CollectionPerson.Filter = FilterToPerson;
        }
    }
    // Список видов паспортов
    private IEnumerable<TypePassport>? _typePassports;
    public IEnumerable<TypePassport>? TypePassports
    {
        get => _typePassports;
        private set => Set(ref _typePassports, value);
    }


    // Список период отпусков
    private IEnumerable<PeriodVacation>? _periodVacation;
    public IEnumerable<PeriodVacation>? PeriodVacation
    {
        get => _periodVacation;
        set => Set(ref _periodVacation, value);
    }

    // Список тип отпусков
    private IEnumerable<Models.TypeVacation>? _typeVacation;
    public IEnumerable<Models.TypeVacation>? TypeVacation
    {
        get => _typeVacation;
        set => Set(ref _typeVacation, value);
    }

    // Список тип родства
    private IEnumerable<TypeFamily>? _typeFamily;
    public IEnumerable<TypeFamily>? TypeFamily
    {
        get => _typeFamily;
        set => Set(ref _typeFamily, value);
    }

    // Список тип пенсионера 
    private IEnumerable<TypePensioner>? _typePensioner;
    public IEnumerable<TypePensioner>? TypePensioner
    {
        get => _typePensioner;
        set => Set(ref _typePensioner, value);
    }
    // Список тип образования 
    private IEnumerable<TypeEducation>? _typeEducation;
    public IEnumerable<TypeEducation>? TypeEducation
    {
        get => _typeEducation;
        set => Set(ref _typeEducation, value);
    }
    // тип награждения
    private IEnumerable<Rewarding>? _TypeRewarding;
    public IEnumerable<Rewarding>? TypeRewarding
    {
        get => _TypeRewarding;
        set => Set(ref _TypeRewarding, value);
    }
    // Приказы для награждения
    private IEnumerable<Rewarding>? _OrderRewarding;
    public IEnumerable<Rewarding>? OrderRewarding
    {
        get => _OrderRewarding;
        set => Set(ref _OrderRewarding, value);
    }
    // Приказы для смены фамилии
    private IEnumerable<OldSurname>? _OrderOldSurname;
    public IEnumerable<OldSurname>? OrderOldSurname
    {
        get => _OrderOldSurname;
        set => Set(ref _OrderOldSurname, value);
    }
    // мед.категории
    private IEnumerable<MedicalCategory>? _MedicalCategory;
    public IEnumerable<MedicalCategory>? MedicalCategory
    {
        get => _MedicalCategory;
        set => Set(ref _MedicalCategory, value);
    }

    private IEnumerable<TypeDegree>? _TypeDegree;
    public IEnumerable<TypeDegree>? TypeDegree
    {
        get => _TypeDegree;
        set => Set(ref _TypeDegree, value);
    }

    // Ученая степень
    private IEnumerable<ScientificDegree>? _ScientificDegree;
    public IEnumerable<ScientificDegree>? ScientificDegree
    {
        get => _ScientificDegree;
        set => Set(ref _ScientificDegree, value);
    }

    // Академик- Член.корр.
    private IEnumerable<TypeRank>? _TypeRanks;
    public IEnumerable<TypeRank>? TypeRanks
    {
        get => _TypeRanks;
        set => Set(ref _TypeRanks, value);
    }
    // Справочник для ученого звания
    private IEnumerable<TypeRank>? _TypeTitle;
    public IEnumerable<TypeRank>? TypeTitle
    {
        get => _TypeTitle;
        private set => Set(ref _TypeTitle, value);
    }

    // Передать значение Отдела
    private Departments? _Department;
    public Departments? Department
    {
        get => _Department;
        set => Set(ref _Department, value);
    }

    // Выбранный отпуск 
    private Vacation? _SelectedVacation;
    public Vacation? SelectedVacation
    {
        get => _SelectedVacation;
        set => Set(ref _SelectedVacation, value);
    }
    // Выбранный стаж
    private HistoryEmployment? _SelectedHistory;
    public HistoryEmployment? SelectedHistory
    {
        get => _SelectedHistory;
        set => Set(ref _SelectedHistory, value);
    }

    private Qualification? _SelectedQualification;
    public Qualification? SelectedQualification
    {
        get => _SelectedQualification;
        set => Set(ref _SelectedQualification, value);
    }

    private Medical? _SelectedMedical;
    public Medical? SelectedMedical
    {
        get => _SelectedMedical;
        set => Set(ref _SelectedMedical, value);
    }

    private MemberAcademic? _SeletedMemberAcademic;
    public MemberAcademic? SeletedMemberAcademic
    {
        get => _SeletedMemberAcademic;
        set => Set(ref _SeletedMemberAcademic, value);
    }
    private AcademicTitle? _SelectedTitle;
    public AcademicTitle? SelectedTitle
    {
        get => _SelectedTitle;
        set => Set(ref _SelectedTitle, value);
    }

    private ScientificDegree? _SeletedDegree;
    public ScientificDegree? SeletedDegree
    {
        get => _SeletedDegree;
        set => Set(ref _SeletedDegree, value);
    }


    // Выбранная персона
    private Persons? _SelectedPerson;
    public Persons? SelectedPerson
    {
        get => _SelectedPerson;
        set => Set(ref _SelectedPerson, value);
    }
    #endregion

    #region Команды
    private ICommand? _UpdateStates;
    public ICommand UpdateStates => _UpdateStates ??= new LambdaCommand(ApiUpdateStatePersonAsync);

    private ICommand? _OpenreportCard;
    public ICommand OpenReportCard => _OpenreportCard ??= new LambdaCommand(ReportPersonCard);

    private ICommand? _OpenAddPosition;
    public ICommand OpenAddPosition => _OpenAddPosition ??= new LambdaCommand(AddPosition , _ => SelectedPerson != null);

    private ICommand? _OpenDropPosition;
    public ICommand OpenDropPosition => _OpenDropPosition ??= new LambdaCommand(DeletePosition, _ => SelectedPerson != null && SelectedPosition != null && SelectedPosition.IsMain != true);

    private ICommand? _OpenChangeSurname;
    public ICommand OpenChangeSurname => _OpenChangeSurname ??= new LambdaCommand(ChangeSurname , _ => SelectedPerson != null);

    private ICommand? _OpenreportSpravka;
    public ICommand OpenreportSpravka => _OpenreportSpravka ??= new LambdaCommand(ReportSpravkaCard, _ => SelectedPosition != null);

    private ICommand? _LoadedListPerson;
    public ICommand LoadedListPerson => _LoadedListPerson ??= new LambdaCommand(ApiGetListPersons);

    private ICommand? _LoadedListAll;
    public ICommand LoadedListAll => _LoadedListAll ??= new LambdaCommand(ApiGetListAllPersonsAsync);

    private ICommand? _UploadPhoto;
    public ICommand UploadPhoto => _UploadPhoto ??= new LambdaCommand(UploadFormPhoto);

    private ICommand? _GetBack;
    public ICommand GetBack => _GetBack ??= new LambdaCommand(GetBackViewAsync);

    private ICommand? _GetInfo;
    public ICommand GetInfo => _GetInfo ??= new LambdaCommand(ApiGetInformationToPerson, CanCommandExecute);

    private ICommand? _GetPersonsToNppAll;
    public ICommand GetPersonsToNppAll => _GetPersonsToNppAll ??= new LambdaCommand(GetNppPersonsAll);

    private ICommand? _GetPersonsToNpp;
    public ICommand GetPersonsToNpp => _GetPersonsToNpp ??= new LambdaCommand(GetNppPersons);

    private ICommand? _GetPersonsToNotNpp;
    public ICommand GetPersonsToNotNpp => _GetPersonsToNotNpp ??= new LambdaCommand(GetNoNppPersons);
    //------------------- Работа ------------------------------//
    // Отпуск
    private ICommand? _AddVacation;
    public ICommand AddVacation => _AddVacation ??= new LambdaCommand(AddVacationsPerson);

    private ICommand? _SaveVacation;
    public ICommand SaveVacation => _SaveVacation ??= new LambdaCommand(SaveVacationsPerson, _ => SelectedVacation != null);

    /*private ICommand? _DeleteVacation;
    public ICommand DeleteVacation => _DeleteVacation ??= new LambdaCommand(DeleteFamilyPerson, _ => SelectedVacation != null);
    */
    //------------------- Паспорт ------------------------------//
    // Родственники
    private ICommand? _AddFamily;
    public ICommand AddFamily => _AddFamily ??= new LambdaCommand(AddFamilyPerson);

    private ICommand? _SaveFamily;
    public ICommand SaveFamily => _SaveFamily ??= new LambdaCommand(SaveFamilyPerson, _ => _selectedFamily != null);

    private ICommand? _DeleteFamily;
    public ICommand DeleteFamily => _DeleteFamily ??= new LambdaCommand(DeleteFamilyPerson, _ => SelectedFamily != null);

    //------------------- ОБРАЗОВАНИЕ ------------------------------//

    // Медицинское образование
    private ICommand? _AddMedical;
    public ICommand AddMedical => _AddMedical ??= new LambdaCommand(AddEducationMed);

    private ICommand? _SaveMedical;
    public ICommand SaveMedical => _SaveMedical ??= new LambdaCommand(SaveEducationMed, _ => SelectedMedical != null);

    private ICommand? _DeleteMedical;
    public ICommand DeleteMedical => _DeleteMedical ??= new LambdaCommand(DeleteEducationMed, _ => SelectedMedical != null);

    // Член-корр.
    private ICommand? _AddMember;
    public ICommand AddMember => _AddMember ??= new LambdaCommand(AddMemberAcademic);

    private ICommand? _SaveMember;
    public ICommand SaveMember => _SaveMember ??= new LambdaCommand(SaveMemberAcademic, _ => SeletedMemberAcademic != null);

    private ICommand? _DeleteMember;
    public ICommand DeleteMember => _DeleteMember ??= new LambdaCommand(DeleteMemberAcademic, _ => SeletedMemberAcademic != null);

    // Награждения
    private ICommand? _AddRewarding;
    public ICommand AddRewarding => _AddRewarding ??= new LambdaCommand(AddRewardingPerson);

    private ICommand? _SaveRewarding;
    public ICommand SaveRewarding => _SaveRewarding ??= new LambdaCommand(SaveRewardingPerson, _ => SelectedRewarding != null);

    private ICommand? _DeleteRewarding;
    public ICommand DeleteRewarding => _DeleteRewarding ??= new LambdaCommand(DeleteRewardingPerson, _ => SelectedRewarding != null);

    // Основное образование
    private ICommand? _AddEducation;
    public ICommand AddEducation => _AddEducation ??= new LambdaCommand(AddMainEducation);

    private ICommand? _SaveEducation;
    public ICommand SaveEducation => _SaveEducation ??= new LambdaCommand(SaveMainEducation, _ => SelectedEducation != null);

    private ICommand? _DeleteEducation;
    public ICommand DeleteEducation => _DeleteEducation ??= new LambdaCommand(DeleteMainEducation, _ => SelectedEducation != null);

    // Повышение квалификации
    private ICommand? _AddQualification;
    public ICommand AddQualification => _AddQualification ??= new LambdaCommand(AddQualificationEducation);

    private ICommand? _SaveQualification;
    public ICommand SaveQualification => _SaveQualification ??= new LambdaCommand(SaveQualificationEducation, _ => SelectedQualification != null);

    private ICommand? _DeleteQualification;
    public ICommand DeleteQualification => _DeleteQualification ??= new LambdaCommand(DeletealificationEducation, _ => SelectedQualification != null);


    // Научная степень
    private ICommand? _AddScience;
    public ICommand AddScience => _AddScience ??= new LambdaCommand(AddScienceDegree);

    private ICommand? _SaveScience;
    public ICommand SaveScience => _SaveScience ??= new LambdaCommand(SaveScienceDegree, _ => SeletedDegree != null);

    private ICommand? _DeleteScience;
    public ICommand DeleteScience => _DeleteScience ??= new LambdaCommand(DeleteScienceDegree, _ => SeletedDegree != null);

    // Ученое звание
    private ICommand? _AddTitle;
    public ICommand AddTile => _AddTitle ??= new LambdaCommand(AddAcademicTitle);

    private ICommand? _SaveTitle;
    public ICommand SaveTile => _SaveTitle ??= new LambdaCommand(SaveAcademicTitle, _ => SelectedTitle != null);

    private ICommand? _DeleteTitle;
    public ICommand DeleteTitle => _DeleteTitle ??= new LambdaCommand(DeleteAcademicTitle, _ => SelectedTitle != null);

    // изменить данные паспорта
    private ICommand? _SaveDataPassport;
    public ICommand SaveDataPassport => _SaveDataPassport ??= new LambdaCommand(UpdatePassport, _ => SelectedPerson != null);

    // Пенсионеры
    private ICommand? _AddPensioner;
    public ICommand AddPensioner => _AddPensioner ??= new LambdaCommand(AddPensionerAsync);

    private ICommand? _SavePensioner;
    public ICommand SavePensioner => _SavePensioner ??= new LambdaCommand(SavePensionerAsync, _ => SelectedPens != null);

    private ICommand? _DeletePensioner;
    public ICommand DeletePensioner => _DeletePensioner ??= new LambdaCommand(DeletePensionerAsync, _ => SelectedPens != null);

    // Инвалиды
    private ICommand? _AddInvalid;
    public ICommand AddInvalid => _AddInvalid ??= new LambdaCommand(AddInvalidAsync);

    private ICommand? _SaveInvalid;
    public ICommand SaveInvalid => _SaveInvalid ??= new LambdaCommand(SaveInvalidAsync, _ => SelectedInvalid != null);

    private ICommand? _DeleteInvalid;
    public ICommand DeleteInvalid => _DeleteInvalid ??= new LambdaCommand(DeleteInvalidAsync, _ => SelectedInvalid != null);

    // Трудовая книга

    private ICommand? _AddHistory;
    public ICommand AddHistory => _AddHistory ??= new LambdaCommand(AddHistoryAsync);

    // Изменить трудовую
    private ICommand? _SaveHistory;
    public ICommand SaveHistory => _SaveHistory ??= new LambdaCommand(SaveHistoryAsync, _ => SelectedHistory != null);



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
                id_person = _SelectedPerson!.Id,
                id_person_position = _selectedPosition!.Id,

            };
            await ReportService.JsonPostWithToken(
                    payload,
                    token: _user!.Token,
                    queryUrl: "/reports/pers/persons/spravka/",
                    HttpMethod: "POST",
                    ReportName: "Справка с места роботы");

            IsLoading = false;

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
                    queryUrl: "/reports/pers/persons/card/" + _SelectedPerson!.Id,
                    HttpMethod: "GET",
                    ReportName: "Личная карта");

            IsLoading = false;

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
            // После получение информации рассчитать стаж
            if (SelectedPerson!.HistoryEmployment?.Count > 0)
            {
                StageIsOver = ServiceWorkingExperience.GetStageIsOver(SelectedPerson.HistoryEmployment);
                StageIsUniver = ServiceWorkingExperience.GetStageIsUniver(SelectedPerson.HistoryEmployment);
                StageIsScience = ServiceWorkingExperience.GetStageIsScience(SelectedPerson.HistoryEmployment);
                StageIsPedagogical = ServiceWorkingExperience.GetStageIsPedagogical(SelectedPerson.HistoryEmployment);
                StageIsMedical = ServiceWorkingExperience.GetStageIsMedical(SelectedPerson.HistoryEmployment);
                StageIsMuseum = ServiceWorkingExperience.GetStageIsMuseum(SelectedPerson.HistoryEmployment);

            }
            IsLoading = false;
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
    public static string GetBase64FromImage(string path)
    {
        using var image = Image.FromFile(path);
        using MemoryStream m = new();
        image.Save(m, image.RawFormat);
        byte[] imageBytes = m.ToArray();

        // Convert byte[] to Base64 String
        var base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }

    // Вернуться на главную страницу
    private async void GetBackViewAsync(object p)
    {
        _Department = await QueryService.JsonDeserializeWithObject<Departments>(token: _user!.Token,
                                                                                "/pers/tree/find/" + _idDepartment,
                                                                                "GET");
        _navigationStore.CurrentViewModel = new HomeViewModel(_user!, _navigationStore, _Department!);
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

    private void ChangeSurname(object p)
    {
        ChangeSurnameViewModel viewModel = new(_user!, SelectedPerson!);
        ChangeSurnameView view = new() { DataContext = viewModel };
        view.ShowDialog();
        ApiGetInformationToPerson(p);
        ApiGetListAllPersonsAsync(p);
        //SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(token: _User!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");
        //PersonsList = await QueryService.JsonDeserializeWithToken<Persons>(token: _User!.Token, "/pers/person/get/all", "GET");


    }

    // Загрузка изображений на человека
    private async void UploadFormPhoto(object p)
    {
        try
        {
            // Задать конфигурацию
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                DefaultExt = ".jpg", // Default file extension
                Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };
            // Если выбран файл
            if (openFileDialog.ShowDialog() != true) return;
            // Разбираем фото на base64 и отправляем на сервер
            var base64 = GetBase64FromImage(openFileDialog.FileName);
            // отправляем base64 
            IsLoading = true;
            // Отправляем base64 image 
            await QueryService.JsonSerializeWithToken(token: _user!.Token,
                "/pers/person/upload/" + SelectedPerson!.Id,
                "PUT",
                new { photo = base64 });

            // Делаем повторный запрос GET
            SelectedPerson = await QueryService.JsonObjectWithToken<Persons>(_user!.Token, "/pers/person/card/" + SelectedPerson!.Id, "GET");

            /*Avatar.BeginInit();
                Avatar.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                Avatar.CacheOption = BitmapCacheOption.OnLoad;
                Avatar.UriSource = new Uri(SelectedPerson.Photo);
                Avatar.EndInit();
                */
                

            IsLoading = false;
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
            PersonsList = await QueryService.JsonDeserializeWithToken<Persons>(token: _user!.Token, "/pers/person/get/all", "GET");

            IsLoading = false;


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
            IsLoading = true;

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
            OrderRewarding = await QueryService.JsonDeserializeWithToken<Rewarding>(token: _user!.Token, "/pers/order/get/9", "GET");
            // Приказы для награждения
            OrderOldSurname = await QueryService.JsonDeserializeWithToken<OldSurname>(token: _user!.Token, "/pers/order/get/5", "GET");
            // Мед.категория 
            MedicalCategory = await QueryService.JsonDeserializeWithToken<MedicalCategory>(token: _user!.Token, "/pers/medical/type/get", "GET");
            // Ученая степень
            TypeDegree = await QueryService.JsonDeserializeWithToken<TypeDegree>(token: _user!.Token, "/pers/scientific/type/get", "GET");
            // Член.корр.
            TypeRanks = await QueryService.JsonDeserializeWithToken<TypeRank>(token: _user!.Token, "/pers/member/type/get", "GET");
            // Тут оставляем всё как есть
            TypeTitle = TypeRanks;
            // Показываем только два элемента для Членов акдемиков
            TypeRanks = TypeRanks.Where(x => x.Name == "Академик" || x.Name == "Член-корреспондент");

            // Получить список людей в отделе
            PersonsList = await QueryService.JsonDeserializeWithToken<Persons>(token: _user!.Token, "/pers/person/get/short/" + _idDepartment, "GET");
            // PersonsList = await _Api.GetListPersonsToDepartment(_User!.Token, _Department!.Id);
            // Маленький костыль , для того чтобы находить на каком item я должен стоять при загрузке личной карты
            (Persons Value, int Index) indeArray;
            indeArray = PersonsList.Select((value, Index) => (Value: value, Index))
                .FirstOrDefault(p => p.Value.Id == SelectedPerson!.Id);
            // Выбрать текущего человека при загрузке Window
            SelectedPerson = indeArray.Value;

            IsLoading = false;
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
    private bool CanCommandExecute(object p) => _SelectedPerson is not null;
    private bool FilterToPerson(object emp) => string.IsNullOrEmpty(FilterPerson) || emp is Persons pers && pers.FirstName!.ToUpper().Contains(FilterPerson.ToUpper());
    private static bool FilterIsNpp(object emp) => emp is Persons pers && pers.IsPed == true;
    private static bool FilterIsNoNpp(object emp) => emp is Persons pers && pers.IsPed == false;

    #endregion

    #region Образование

    /* Основное образование */
    private async void AddMainEducation(object p)
    {
        try
        {
            Education order = new()
            {
                TypeName = "Бакалавр",
                IsActual = true,
                DateIssue = DateTime.Now,
                Institution = "",

            };
            _SelectedPerson!.ArrayEducation?.Insert(0, order);
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
            if (_user!.Token == null) return;

            SelectedEducation!.IdPerson = SelectedPerson!.Id;

            if (SelectedEducation!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/education/rename/", "POST", SelectedEducation);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/education/add", "POST", SelectedEducation);
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/education/del/" + SelectedEducation!.Id, "DELETE", SelectedEducation);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _SelectedPerson!.ArrayEducation!.Remove(SelectedEducation);
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
            Qualification order = new()
            {
                NameCourse = "Курс",
                Certificate = "№ 0000",
                DateBegin = DateTime.Now,
                DateEnd = DateTime.Now,
                DateIssue = DateTime.Now,
                Place = "Место выдачи"

            };
            _SelectedPerson!.ArrayQualification?.Insert(0, order);
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
            if (_user!.Token == null) return;

            SelectedQualification!.IdPerson = SelectedPerson!.Id;

            if (SelectedQualification!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/qualification/rename/", "POST", SelectedQualification);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/qualification/add", "POST", SelectedQualification);
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/qualification/del/" + SelectedQualification!.Id, "DELETE", SelectedQualification);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _SelectedPerson!.ArrayQualification!.Remove(SelectedQualification);
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
            Medical order = new()
            {
                Category = "Аттестация",
                Name = "Новый элемент",
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
            };
            _SelectedPerson!.ArrayMedical?.Insert(0, order);
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
            if (_user!.Token == null) return;

            SelectedMedical!.IdPerson = SelectedPerson!.Id;

            if (SelectedMedical!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/medical/rename/", "POST", SelectedMedical);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/medical/add", "POST", SelectedMedical);
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

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/medical/del/" + SelectedMedical!.Id, "DELETE", SelectedMedical);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _SelectedPerson!.ArrayMedical!.Remove(SelectedMedical);
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
            MemberAcademic order = new()
            {
                Document = "№ 0000",
                NameAcademic = "",
                Datebegin = DateTime.Now,
                Name = "Академик",

            };
            _SelectedPerson!.ArrayMeberAcademic?.Insert(0, order);
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
            if (_user!.Token == null) return;

            SeletedMemberAcademic!.IdPerson = SelectedPerson!.Id;

            if (SeletedMemberAcademic!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/member/rename/", "POST", SeletedMemberAcademic);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/member/add", "POST", SeletedMemberAcademic);
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/member/del/" + SeletedMemberAcademic!.IdMember, "DELETE", SeletedMemberAcademic);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _SelectedPerson!.ArrayMeberAcademic!.Remove(SeletedMemberAcademic);
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
            Rewarding order = new()
            {
                NumberDocumet = "",
                Type = "Благодарность",
                DateIssue = DateTime.Now,


            };
            _SelectedPerson!.ArrayRewarding?.Insert(0, order);
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
            if (_user!.Token == null) return;

            SelectedRewarding!.IdPerson = SelectedPerson!.Id;

            //SelectedRewarding!.IdOrder = 

            if (SelectedRewarding!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rewarding/rename/", "POST", SelectedRewarding);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rewarding/add", "POST", SelectedRewarding);
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rewarding/del/" + SelectedRewarding!.Id, "DELETE", SelectedRewarding);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _SelectedPerson!.ArrayRewarding!.Remove(SelectedRewarding);
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
            ScientificDegree order = new()
            {
               DateOfIssue = DateTime.Now,
               Document = "№ 0000",
               City = "Город",
               Place = "Место",
               ScientificBranch = "Отрасль",
               ScientificSpecialty = "Специальность"

            };
            _SelectedPerson!.ArrayScientificDegree?.Insert(0, order);
            SeletedDegree = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
            AcademicTitle order = new()
            {
                Department = "Наименование подразделения",
                Document = "№ 0000",
                Place = "Место присвоения",
                DateIssue = DateTime.Now,

            };
            _SelectedPerson!.ArrayAcademicTitle?.Insert(0, order);
            SelectedTitle = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
            if (_user!.Token == null) return;

            SeletedDegree!.IdPerson = SelectedPerson!.Id;

            //SelectedRewarding!.IdOrder = 

            if (SeletedDegree!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/scientific/rename", "POST", SeletedDegree);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/scientific/add", "POST", SeletedDegree);
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
            if (_user!.Token == null) return;

            SelectedTitle!.IdPerson = SelectedPerson!.Id;

            //SelectedRewarding!.IdOrder = 

            if (SelectedTitle!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rank/rename/", "POST", SelectedTitle);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rank/add", "POST", SelectedTitle);
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/scientific/del/" + SeletedDegree!.Id, "DELETE", SeletedDegree);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _SelectedPerson!.ArrayScientificDegree!.Remove(SeletedDegree);
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            await QueryService.JsonSerializeWithToken(_user.Token, "/pers/rank/del/" + SelectedTitle!.Id, "DELETE", SelectedTitle);
            //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

            _ = _SelectedPerson!.ArrayAcademicTitle!.Remove(SelectedTitle);
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

    // Сохранить изменения паспорта
    private async void UpdatePassport(object p)
    {
        try
        {
            if (_user!.Token == null) return;

            if (SelectedPerson!.SerialPassport.Length == 0 || SelectedPerson!.SerialPassport.Length > 12)
            {
                _ = MessageBox.Show("Не допустимый размер поля 'Серия паспорта'");
                return;
            }

            if (SelectedPerson!.NumberPassport.Length == 0 || SelectedPerson!.NumberPassport.Length > 12)
            {
                _ = MessageBox.Show("Не допустимый размер поля 'Номер паспорта'");
                return;
            }

            if (SelectedPerson!.Id > 0)
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/rename/passport", "POST", SelectedPerson);

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

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Пенсионер
    private async void AddPensionerAsync(object p)
    {
        try
        {
            Pensioner order = new()
            {
                DateDocument = DateTime.Now,
                Document = "№ 0000",

            };
            _SelectedPerson!.ArrayPensioner?.Insert(0, order);
            SelectedPens = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SavePensionerAsync(object p)
    {
        try
        {
            if (_user!.Token == null) return;

            SelectedPens!.IdPerson = SelectedPerson!.Id;

            if (SelectedPens!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/pensioner/rename", "POST", SelectedPens);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/pensioner/add", "POST", SelectedPens);
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

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/pensioner/del/" + SelectedPens!.Id, "DELETE", SelectedPens);
                //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

                _ = _SelectedPerson!.ArrayPensioner!.Remove(SelectedPens);
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Инвалидность
    private async void AddInvalidAsync(object p)
    {
        try
        {
            Invalid order = new()
            {
                DateStart = DateTime.Now,
                Document = "№ 0000",
                Group = "1"


            };
            _SelectedPerson!.ArrayInvalid?.Insert(0, order);
            SelectedInvalid = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private async void SaveInvalidAsync(object p)
    {
        try
        {
            if (_user!.Token == null) return;

            SelectedInvalid!.IdPerson = SelectedPerson!.Id;

            if (SelectedInvalid!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/invalid/rename", "POST", SelectedInvalid);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/invalid/add", "POST", SelectedInvalid);
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

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/invalid/" + SelectedInvalid!.Id, "DELETE", SelectedInvalid);
                //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

                _ = _SelectedPerson!.ArrayInvalid!.Remove(SelectedInvalid);
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Отпуск
    private async void AddVacationsPerson(object p)
    {
        try
        {
            Vacation order = new()
            {
                DateBegin = DateTime.Now,
                DateEnd = DateTime.Now,
                LengthVacation = 0,
                Residue = 0,
                Type = ""

            };
            _SelectedPerson!.ArrayVacation?.Insert(0, order);
            SelectedVacation = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Статус семьи
    private async void AddFamilyPerson(object p)
    {
        try
        {
            Family order = new()
            {
                FullName = "ФИО",
                Birthday = DateTime.Now,
                Description = "Примечание",

            };
            _SelectedPerson!.ArrayFamily?.Insert(0, order);
            SelectedFamily = order;

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Не удалось получить данные с API!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void SaveVacationsPerson(object p)
    {
        try
        {
            if (_user!.Token == null) return;

            SelectedVacation!.IdPerson = SelectedPerson!.Id;

            if (SelectedVacation!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/update", "POST", SelectedVacation);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/vacation/add", "POST", SelectedVacation);
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

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
            if (_user!.Token == null) return;

            SelectedFamily!.IdPerson = SelectedPerson!.Id;

            if (SelectedFamily!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/family/rename", "POST", SelectedFamily);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/family/add", "POST", SelectedFamily);
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

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
            if (_user!.Token == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотитет удалить данный отдел?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/family/" + SelectedFamily!.Id, "DELETE", SelectedFamily);
                //_Api.DeleteDepartment(_User.Token, SelectedDepartment.Id);

                _ = _SelectedPerson!.ArrayFamily!.Remove(SelectedFamily);
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());

                    if (reader != null)
                    {
                        _ = MessageBox.Show(await reader.ReadToEndAsync(), "Ошибочка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
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
    private async void AddHistoryAsync(object p)
    {
        try
        {
            HistoryEmployment order = new()
            {
                CreateAt = DateTime.Now,
                Name = "Наименование",
                Record = 0

            };
            _SelectedPerson!.HistoryEmployment?.Insert(0, order);
            SelectedHistory = order;

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

    private async void SaveHistoryAsync(object p)
    {
        try
        {
            if (_user!.Token == null) return;

            SelectedHistory!.IdPerson = SelectedPerson!.Id;

            if (SelectedHistory!.Id > 0)
            {
                // Изменить
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/history/rename/" + SelectedHistory.Id, "POST", SelectedHistory);
            }
            else
            {
                // Создать
                await QueryService.JsonSerializeWithToken(_user.Token, "/pers/person/history/add", "POST", SelectedHistory);
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

    #endregion

    public override void Dispose()
    {

        base.Dispose();
    }

}

