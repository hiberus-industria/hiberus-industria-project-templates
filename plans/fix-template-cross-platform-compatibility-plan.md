## Plan: Fix Template Cross-Platform Compatibility

El template Aspire React está fallando en CI (Linux) debido a rutas con backslashes y configuración incompleta del template.json. Este plan corrige la compatibilidad cross-platform y asegura que el template se instancie correctamente, siguiendo las prácticas de Microsoft Aspire.

**Phases: 5**

1. **Phase 1: Estandarizar Rutas a Forward Slashes en Archivos de Build**
    - **Objective:** Corregir todas las rutas en archivos .csproj y configuración VS Code para usar forward slashes, garantizando compatibilidad cross-platform
    - **Files/Functions to Modify/Create:**
        - [Hiberus.Industria.Templates.csproj](Hiberus.Industria.Templates.csproj#L25-L30) - Cambiar PackagePath de `content\templates\` a `content/templates/`
        - [templates/Hiberus.Industria.Templates.Aspire.React/.vscode/launch.json](templates/Hiberus.Industria.Templates.Aspire.React/.vscode/launch.json#L11) - Cambiar backslashes a forward slashes en projectPath
    - **Tests to Write:**
        - Verificar que `dotnet pack` se ejecuta exitosamente en PowerShell
        - Extraer el .nupkg y verificar que la estructura de directorios es `content/templates/`
        - Verificar que el archivo launch.json usa forward slashes
    - **Steps:**
        1. En [Hiberus.Industria.Templates.csproj](Hiberus.Industria.Templates.csproj), cambiar todos los backslashes a forward slashes en Include, Exclude y PackagePath
        2. En [.vscode/launch.json](templates/Hiberus.Industria.Templates.Aspire.React/.vscode/launch.json), cambiar backslashes a forward slashes en projectPath
        3. Ejecutar `dotnet pack -o artifacts` y verificar que se crea el paquete sin errores
        4. Extraer el .nupkg y verificar la estructura de directorios usando PowerShell
        5. Confirmar que todos los archivos del template están en `content/templates/`

2. **Phase 2: Estandarizar ProjectReferences en Archivos .csproj del Template**
    - **Objective:** Cambiar ProjectReferences con backslashes a forward slashes en todos los .csproj del template Aspire React
    - **Files/Functions to Modify/Create:**
        - [templates/Hiberus.Industria.Templates.Aspire.React/src/Services/Hiberus.Industria.Templates.Aspire.React.Server/Hiberus.Industria.Templates.Aspire.React.Server.csproj](templates/Hiberus.Industria.Templates.Aspire.React/src/Services/Hiberus.Industria.Templates.Aspire.React.Server/Hiberus.Industria.Templates.Aspire.React.Server.csproj#L13-L15) - Cambiar `..\..\` a `../../`
    - **Tests to Write:**
        - Verificar que todos los .csproj usan forward slashes en ProjectReferences usando grep
        - Re-empaquetar el template y verificar que no hay backslashes en archivos .csproj
    - **Steps:**
        1. Usar grep para encontrar todos los ProjectReferences con backslashes: `grep -r "ProjectReference.*\\\\" templates/`
        2. Cambiar todas las referencias en [Server.csproj](templates/Hiberus.Industria.Templates.Aspire.React/src/Services/Hiberus.Industria.Templates.Aspire.React.Server/Hiberus.Industria.Templates.Aspire.React.Server.csproj) de `..\..\` a `../../`
        3. Ejecutar `grep -r "ProjectReference.*\\\\" templates/` nuevamente y confirmar que no hay resultados
        4. Re-empaquetar el template con `dotnet pack`
        5. Confirmar que el comando pack se ejecuta sin errores

3. **Phase 3: Corregir Configuración de template.json para Renombrado de Archivos**
    - **Objective:** Añadir símbolo para renombrar el archivo .slnx y el realm file de Keycloak, asegurando que se instancien con el nombre del proyecto
    - **Files/Functions to Modify/Create:**
        - [templates/Hiberus.Industria.Templates.Aspire.React/.template.config/template.json](templates/Hiberus.Industria.Templates.Aspire.React/.template.config/template.json#L21-L25) - Añadir fileRename al símbolo appSlug para el archivo realm
        - Renombrar [templates/Hiberus.Industria.Templates.Aspire.React/Hiberus.Industria.Templates.Aspire.React.slnx](templates/Hiberus.Industria.Templates.Aspire.React/Hiberus.Industria.Templates.Aspire.React.slnx) a `Templates.Aspire.React.slnx` para coincidir con sourceName
    - **Tests to Write:**
        - Instanciar el template con `dotnet new aspire-react -n TestProject -o test-output` y verificar que:
            - El archivo .slnx se renombra a `TestProject.slnx`
            - El archivo realm se renombra a `testproject-realm.json`
    - **Steps:**
        1. Analizar el sourceName actual ("Templates.Aspire.React") y verificar patrones de renombrado
        2. Renombrar el archivo .slnx de `Hiberus.Industria.Templates.Aspire.React.slnx` a `Templates.Aspire.React.slnx` para coincidir con sourceName
        3. Renombrar el realm file de `templates-aspire-react-realm.json` a `templates-aspire-react-realm.json` (ya coincide con appSlug)
        4. Actualizar [template.json](templates/Hiberus.Industria.Templates.Aspire.React/.template.config/template.json) para añadir fileRename al realm file si es necesario
        5. Re-empaquetar con `dotnet pack`
        6. Instalar el template con `dotnet new install artifacts/Hiberus.Industria.Templates.*.nupkg`
        7. Instanciar el template en test-output y verificar nombres de archivos

4. **Phase 4: Ejecutar Test Script y Validar Instanciación del Template**
    - **Objective:** Ejecutar el script test-templates.sh completo y verificar que todos los tests pasan, incluyendo build y validaciones estructurales
    - **Files/Functions to Modify/Create:**
        - Ninguno (solo validación)
    - **Tests to Write:**
        - Ejecutar `bash scripts/test-templates.sh aspire-react` desde el directorio raíz
        - Verificar que todos los checks pasan:
            - ✓ Estructura de proyecto correcta
            - ✓ sourceName replacement funciona
            - ✓ No hay placeholders sin reemplazar
            - ✓ dotnet restore funciona
            - ✓ dotnet build funciona
    - **Steps:**
        1. Limpiar artifacts previos: `rm -rf test-output artifacts/*.nupkg`
        2. Re-empaquetar el template: `dotnet pack -o artifacts`
        3. Desde el directorio raíz, ejecutar: `bash scripts/test-templates.sh aspire-react`
        4. Revisar la salida completa del script y verificar que todos los checks (✓) pasan
        5. Si hay errores, identificar la causa específica y documentar
        6. Confirmar que `dotnet build` en el proyecto instanciado termina exitosamente

5. **Phase 5: Validar CI Workflow Localmente**
    - **Objective:** Simular el workflow del CI localmente para confirmar que todos los pasos funcionan en un entorno limpio
    - **Files/Functions to Modify/Create:**
        - Ninguno (solo validación)
    - **Tests to Write:**
        - Ejecutar cada paso del [CI workflow](.github/workflows/ci.yml) manualmente:
            - Validar configuración con `bash scripts/validate-template-config.sh`
            - Empaquetar con `dotnet pack`
            - Ejecutar tests con `bash scripts/test-templates.sh`
    - **Steps:**
        1. Limpiar workspace: `git clean -xdf artifacts/ test-output/`
        2. Ejecutar script de validación: `bash scripts/validate-template-config.sh`
        3. Verificar que no hay errores de validación
        4. Empaquetar: `dotnet pack -o artifacts`
        5. Ejecutar tests: `bash scripts/test-templates.sh aspire-react`
        6. Confirmar que todos los pasos del CI simulado pasan exitosamente
        7. Revisar que no quedan archivos temporales o artefactos no deseados

**Open Questions:**

1. **sourceName**: ¿Debemos cambiar el sourceName de "Templates.Aspire.React" a "Hiberus.Industria.Templates.Aspire.React" para coincidir exactamente con los nombres de carpetas/archivos, o mantener el actual y renombrar archivos?
2. **Realm file renaming**: ¿El archivo realm de Keycloak debe renombrarse automáticamente al instanciar el template, o debe mantener un nombre fijo?
3. **Launch.json**: ¿Debemos incluir el archivo .vscode/launch.json en el template, o debe estar en .gitignore/.templateignore?
4. **Test en Windows**: ¿Necesitamos también ejecutar los tests en PowerShell/Windows para garantizar compatibilidad bidireccional, o solo en bash es suficiente?
5. **Template Angular**: ¿El template de Angular tiene los mismos problemas o está pendiente de implementación completa?
