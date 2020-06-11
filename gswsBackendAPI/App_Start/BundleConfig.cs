using System.Web;
using System.Web.Optimization;

namespace gswsBackendAPI
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

//			bundles.Add(new StyleBundle("~/assets/css").Include(
//				   "~/assets/css/icons/icomoon/styles.css",
//"~/assets/css/icons/material/icons.css",
//"~/assets/css/bootstrap.css",
//"~/assets/css/bootstrap_limitless.css",
//"~/assets/css/layout.css",
//"~/assets/css/components.css",
//"~/assets/css/colors.css",
//"~/assets/css/slick.css",
//"~/assets/css/slick-theme.css",
//"~/assets/css/style.css"));

//			bundles.Add(new ScriptBundle("~/assets/js").Include(
//								"~/assets/js/jquery.min.js",
//			"~/assets/js/bootstrap.bundle.min.js",
//			"~/assets/js/ripple.min.js",
//			"~/assets/plugins/uniform.min.js",
//			"~/assets/plugins/switch.min.js"));


//			bundles.Add(new ScriptBundle("~/JS_Modules/Libraries").Include(
//						 "~/JS_Modules/Libraries/angular.min.js",
//		"~/JS_Modules/config.js",
//		"~/Depts/SocialWelfare_Tribal/UI/config/Common_Validations.js",
//		"~/js/JS_Aadhar_Verifivation.js",
//		"~/JS_Modules/Libraries/SweetAlert2.js",
//		"~/JS_Modules/Libraries/angular.ui.route.min.js",
//		"~/JS_Modules/Libraries/angular.filter.min.js",
//		"~/JS_Modules/Libraries/angular.upload.js",
//		"~/JS_Modules/Libraries/network.js",
//		"~/JS_Modules/Libraries/InputMasking.js",
//		"~/JS_Modules/Libraries/DataTables/angular-datatables.bootstrap.js",
//		"~/JS_Modules/Libraries/DataTables/angular-datatables.directive.js",
//		"~/JS_Modules/Libraries/DataTables/angular-datatables.factory.js",
//		"~/JS_Modules/Libraries/DataTables/angular-datatables.js"
//							));

//			bundles.Add(new ScriptBundle("~/UI").Include(
//								   "~/UI/loginController.js",
//					"~/UI/LoginService.js",
//					"~/UI/MainController.js",
//					"~/UI/ulbtelugu.js",
//					"~/UI/temp.js",
//					"~/UI/TestMainController.js",
//					"~/UI/DashboardController.js",
//					"~/UI/GVDashboard.js",
//					"~/UI/ui.js",
//					"~/UI/TransRespController.js",
//					"~/UI/ReceivedController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/SocialWelfare_Tribal").Include(
//			  "~/Depts/SocialWelfare_Tribal/UI/config/Cert_Down_Route.js",
//	"~/Depts/SocialWelfare_Tribal/UI/config/SocialWelfareServices.js",
//	"~/Depts/SocialWelfare_Tribal/UI/Controllers/YPK_Mrg_Cert_Download.js",
//	"~/Depts/SocialWelfare_Tribal/UI/Controllers/Status_Check.js",
//	"~/Depts/SocialWelfare_Tribal/UI/Controllers/Education_Scholarships_check.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Endowments").Include(
//	"~/Depts/Endowments/UI/Config/EndowmentsServices.js",
//	"~/Depts/Endowments/UI/config/EndowmentsRoutes.js",
//	"~/Depts/Endowments/UI/Controllers/HomeController.js",
//	"~/Depts/Endowments/UI/Controllers/ABWCStatusController.js"));




//			bundles.Add(new ScriptBundle("~/Depts/RTGS").Include(
//			"~/Depts/RTGS/UI/Config/RTGSServices.js",
//			"~/Depts/RTGS/UI/config/RTGSRoutes.js",
//			"~/Depts/RTGS/UI/Controllers/HomeController.js",
//			"~/Depts/RTGS/UI/Controllers/PSSStatusCotroller.js",
//			"~/Depts/RTGS/UI/Controllers/PSSUNSurveyController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Education").Include(
//	"~/Depts/Education/UI/config/EducationServices.js",
//	"~/Depts/Education/UI/config/EducationRoutes.js",
//	"~/Depts/Education/UI/Controllers/AmmavodiContoller.js",
//	"~/Depts/Education/UI/Controllers/EducationController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/SERP").Include(
//		"~/Depts/SERP/UI/Config/SERPServices.js",
//		"~/Depts/SERP/UI/config/SERPRoutes.js",
//		"~/Depts/SERP/UI/Controllers/HomeController.js",
//		"~/Depts/SERP/UI/Controllers/PensionStatusController.js",
//		"~/Depts/SERP/UI/Controllers/SerpBimaController.js",
//		"~/Depts/SERP/UI/Controllers/SerpBimaRegsitrationController.js",
//		"~/Depts/SERP/UI/Controllers/BLSVerificationController.js",
//		"~/Depts/SERP/UI/Controllers/SRSVerificationController.js",
//		"~/Depts/SERP/UI/Controllers/PensionApplication.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Health").Include(
//"~/Depts/Health/UI/config/HealthServices.js",
//"~/Depts/Health/UI/config/HealthRoutes.js",
//"~/Depts/Health/UI/Controllers/ArogyaRakshaController.js",
//"~/Depts/Health/UI/Controllers/SadaremCertificate.js",
//"~/Depts/Health/UI/Controllers/YSRKantiVeluguController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Services").Include(
//			"~/Depts/Services/UI/config/SevicesServices.js",
//			"~/Depts/Services/UI/config/ServicesRoutes.js",
//			"~/Depts/Services/UI/Controllers/NPCIController.js",
//			"~/Depts/Services/UI/Controllers/Certificate_Controller.js"));


//			bundles.Add(new ScriptBundle("~/Depts/REVENUE").Include(
//			"~/Depts/REVENUE/UI/Config/REVENUEServices.js",
//			"~/Depts/REVENUE/UI/config/REVENUERoutes.js",
//			"~/Depts/REVENUE/UI/Controllers/RationStatusController.js",
//			"~/Depts/REVENUE/UI/Controllers/RationReg.js",
//			"~/Depts/REVENUE/UI/Controllers/Revenue_ExciseComplaints.js",
//			"~/Depts/REVENUE/UI/Controllers/Excise_ComplaintStatuscheck.js"));

//			bundles.Add(new ScriptBundle("~/Internal/UI").Include(
//			  "~/Internal/UI/Config/InternalRoutes.js",

//			  "~/Internal/UI/Config/InternalServices.js",
//			  "~/Internal/UI/Controllers/InternalController.js",
//			  "~/Internal/UI/Controllers/URLUpdation.js",
//			  "~/Internal/UI/Controllers/SecretriatMasterjs.js"));

//			bundles.Add(new ScriptBundle("~/Depts/MAUD").Include(
//			"~/Depts/MAUD/UI/config/Maud_Route.js",
//			"~/Depts/MAUD/UI/config/MAUD_Services.js",
//			"~/Depts/MAUD/UI/Controllers/MAUD_App_Check.js",
//			"~/Depts/MAUD/UI/Controllers/MAUDJS.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Minority").Include(
//			"~/Depts/Minority/UI/config/MinorityRoutes.js",
//			"~/Depts/Minority/UI/config/MinorityServices.js",
//			"~/Depts/Minority/UI/Controllers/WomenDivorcedStatus.js",
//			"~/Depts/Minority/UI/Controllers/ImamAndMouzansDetails.js"));

//			bundles.Add(new ScriptBundle("~/MeesevaService/UI").Include(
//			"~/MeesevaService/UI/config/Meesevaroute.js",
//			"~/MeesevaService/UI/config/meesevaservice.js",
//			"~/MeesevaService/UI/controller/MeesevaController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Transport_RandB").Include(
//			"~/Depts/Transport_RandB/UI/Config/TransportRoutes.js",
//			"~/Depts/Transport_RandB/UI/Config/TransportServices.js",
//			"~/Depts/Transport_RandB/UI/Controllers/RandB_ComplaintappSearch.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Women_Children_Disabled_and_SeniorCitizen/UI").Include(
//			"~/Depts/Women_Children_Disabled_and_SeniorCitizen/UI/Config/Women_Route.js",
//			"~/Depts/Women_Children_Disabled_and_SeniorCitizen/UI/Config/Women_services.js",
//			"~/Depts/Women_Children_Disabled_and_SeniorCitizen/UI/Controllers/APDASCAC_Regform.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Home").Include(
//			"~/Depts/Home/UI/config/homeRoutes.js",
//			"~/Depts/Home/UI/config/HomeServices.js",
//			"~/Depts/Home/UI/Controllers/homeController.js",
//			"~/Depts/Home/UI/Controllers/Recover_VehicleSearch.js",
//			"~/Depts/Home/UI/Controllers/PetitionCheck.js",
//			"~/Depts/Home/UI/Controllers/KnowCaseStatus.js",
//			"~/Depts/Home/UI/Controllers/ViewFIR.js",
//			"~/Depts/Home/UI/Controllers/ArrestParticulars.js",
//			"~/Depts/Home/UI/Controllers/UnknownBodies.js",
//			"~/Depts/Home/UI/Controllers/Missed_Kidnapped_persons.js",
//			"~/Depts/Home/UI/Controllers/WantedCriminals.js"));

//			bundles.Add(new ScriptBundle("~/Depts/AgriCulture").Include(
//			"~/Depts/AgriCulture/UI/config/agricultureRoutes.js",
//			"~/Depts/AgriCulture/UI/config/agricultureServices.js",
//			"~/Depts/AgriCulture/UI/Controllers/SeedEligibility.js",
//			"~/Depts/AgriCulture/UI/Controllers/EligibleBenificiaries.js",
//			"~/Depts/AgriCulture/UI/Controllers/GetVAAdetails.js",
//			"~/Depts/AgriCulture/UI/Controllers/VillageProfile.js"));

//			bundles.Add(new ScriptBundle("~/Depts/PRRD").Include(
//			"~/Depts/PRRD/UI/config/PRRDRoutes.js",
//			"~/Depts/PRRD/UI/config/PRRDServices.js",
//			"~/Depts/PRRD/UI/controller/TaxSearch.js",
//			"~/Depts/PRRD/UI/controller/JobCardPayment.js",
//			"~/Depts/PRRD/UI/controller/FarmerData.js",
//			"~/Depts/PRRD/UI/controller/DemandCapture.js",
//			"~/Depts/PRRD/UI/controller/ConfirmDemand.js",
//			"~/Depts/PRRD/UI/controller/BuildingPlanApplicationStatus.js",
//			"~/Depts/PRRD/UI/controller/LayoutPlanApplicationStatus.js",
//			"~/Depts/PRRD/UI/controller/LayoutPlanInformation.js",
//			"~/Depts/PRRD/UI/controller/TransportDrinkingWater.js",
//			"~/Depts/PRRD/UI/controller/MarriageApplication.js",
//			"~/Depts/PRRD/UI/controller/NOCApplication.js",
//			"~/Depts/PRRD/UI/controller/VolunteerMapping.js",
//			"~/Depts/PRRD/UI/controller/KnowYourVolunteer.js"));

//			bundles.Add(new ScriptBundle("~/Depts/YATC").Include(
//			"~/Depts/YATC/UI/config/YATCRoutes.js",
//			"~/Depts/YATC/UI/config/YATCServices.js",
//			"~/Depts/YATC/UI/Controllers/CandidateRegController.js",
//			"~/Depts/YATC/UI/Controllers/CanLoginController.js",
//			"~/Depts/YATC/UI/Controllers/ShowAllJobsController.js",
//			"~/Depts/YATC/UI/Controllers/UpCommingBatchesController.js",
//			"~/Depts/YATC/UI/Controllers/StatusCheckController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Energy").Include(
//			"~/Depts/Energy/UI/config/EnergyRoutes.js",
//			"~/Depts/Energy/UI/config/EnergyServices.js",
//			"~/Depts/Energy/UI/Controllers/APSPDCLServiceStatusController.js",
//			"~/Depts/Energy/UI/Controllers/APSPDCLServiceHistoryController.js",
//			"~/Depts/Energy/UI/Controllers/APEPDCLServiceStatusController.js",
//			"~/Depts/Energy/UI/Controllers/APEPDCLServiceHistoryController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/Industries").Include(
//			"~/Depts/Industries/UI/config/IndustriesRoutes.js",
//			"~/Depts/Industries/UI/config/IndustriesServices.js",
//			"~/Depts/Industries/UI/Controllers/YSRNavodayamOTRRegController.js"));

//			bundles.Add(new ScriptBundle("~/Depts/CommercialTax").Include(
//			"~/Depts/CommercialTax/UI/config/CTServices.js",
//			"~/Depts/CommercialTax/UI/config/CTRoutes.js",
//			"~/Depts/CommercialTax/UI/Controllers/PTRegistrationControllers.js",
//			"~/Depts/CommercialTax/UI/Controllers/PTStatusCheckController.js"));
//				BundleTable.EnableOptimizations = true;
		}
	}
}
