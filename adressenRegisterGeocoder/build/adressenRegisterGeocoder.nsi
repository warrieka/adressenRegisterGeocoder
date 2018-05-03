;NSIS Script to create the installation package to deploy adressenRegisterGeocoder on windows
;Written by Kay Warrie

;--------------------------------
;Includes
  !include "MUI2.nsh"
  !include "DotNetChecker.nsh"  ;requeres dll https://github.com/ReVolly/NsisDotNetChecker 
 
;--------------------------------
;General

  ;Name and file
  Name "Adressen Register Geocoder"
  OutFile "adressenRegisterGeocoder.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\adressenRegisterGeocoder"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\adressenRegisterGeocoder" "Install_Dir"

  ;Request application privileges 
  RequestExecutionLevel highest

  !define APPNAME "AdressenRegisterGeocoder"
  !define COMPANYNAME "KayWarrie"
  
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING
  !define MUI_WELCOMEPAGE_TITLE "Adressen Register Geocoder" 
  !define MUI_ICON "..\geopuntAddress.ico"
  !define MUI_WELCOMEPAGE_TEXT "Laat toe om adressen te geocoderen op basis van de adressen match service van adressen register"
 
;--------------------------------
;Pages
  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "..\License.txt" 
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
  !insertmacro MUI_LANGUAGE "Dutch"


  
;--------------------------------
;Installer Sections
Section ;always executed, is hidden for user
 ;dotnet
 !insertmacro CheckNetFramework 45
  
 ;Store installation folder
 WriteRegStr HKCU "Software\LNE_QGIS_tools" "" $INSTDIR
 
 ;create the output dir
 SetOutPath $INSTDIR
 File "..\geopuntAddress.ico"
 
 ;add entry as software 
 WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME}_${APPNAME}" "DisplayName" "Adressen Register Geocoder"
 WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME}_${APPNAME}" "DisplayIcon" "$INSTDIR\geopuntAddress.ico"
 WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME}_${APPNAME}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
 WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME}_${APPNAME}" "HelpLink" "https://github.com/warrieka/adressenRegisterGeocoder"
 WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME}_${APPNAME}" "Publisher" ${COMPANYNAME}

 File /r "..\bin\Release\*"
  
 ;Store installation folder
 WriteRegStr HKCU "Software\adressenRegisterGeocoder" "" $INSTDIR
  
 ;Create uninstaller
 WriteUninstaller "$INSTDIR\Uninstall.exe"
 
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts" startMenu
  CreateDirectory "$SMPROGRAMS\${APPNAME}"
  CreateShortCut "$SMPROGRAMS\${APPNAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\${APPNAME}\${APPNAME}.lnk" "$INSTDIR\adressenRegisterGeocoder.exe" "" "$INSTDIR\adressenRegisterGeocoder.exe" 0
SectionEnd

; Optional section  create desktop shortcut
Section "Desktop Shortcuts" desktop
  CreateShortCut "$DESKTOP\${APPNAME}.lnk" "$INSTDIR\adressenRegisterGeocoder.exe" "" "$INSTDIR\adressenRegisterGeocoder.exe" 0
SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir /r "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\adressenRegisterGeocoder"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME}_${APPNAME}"
  
;Delete Start Menu Shortcuts
  Delete "$DESKTOP\${APPNAME}.lnk"
  Delete  "$SMPROGRAMS\${APPNAME}\${APPNAME}.lnk"
  RmDir /r  "$SMPROGRAMS\${APPNAME}"
  

SectionEnd