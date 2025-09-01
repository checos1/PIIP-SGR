(function () {
    'use strict';

    angular.module('backbone')
        .controller('modalusoController', modalusoController);

    modalusoController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModalInstance',
        'modalusoServicio',
        'utilidades'
    ];

    function modalusoController(
        $scope,
        $sessionStorage,
        $uibModalInstance,
        modalusoServicio,
        utilidades
    ) {
        const vm = this;
        vm.dataEdit = "";

        // Objeto para almacenar todos los datos del formulario
        vm.formData = {            
            Id: "",
            TipoDocumentoId: "",
            TipoTramiteId: "",
            Roles: [],
            IdFase: "",
            Obligatorio: false,
            Visible: false,
            ValidacionId: "",
            ObligatoriedadId: "",
        };

        /**
         * Control Sayayin
         * */

        // Lista de roles seleccionados
        vm.selectedRoles = []; // Lista de objetos seleccionados temporalmente
        vm.searchRoleText = "";
        vm.isDropdownOpen = false;
        vm.activeTab = 'listado';

        // Alternar el dropdown
        vm.toggleDropdown = function () {
            vm.isDropdownOpen = !vm.isDropdownOpen;
        };

        // Cambiar entre pestañas
        vm.setTab = function (tab) {
            vm.activeTab = tab;
        };

        // Actualizar la selección
        vm.updateSelection = function (role) {
             if (role.selected) {
                if (!vm.formData.Roles.includes(role)) {
                    vm.formData.Roles.push(role);                    
                }
            } else {
                vm.formData.Roles = vm.formData.Roles.filter(r => r.IdRol !== role.IdRol);
            }
        };
       
        // Limpiar todas las selecciones
        vm.clearSelection = function () {
            vm.Referencias.Roles.forEach(role => role.selected = false);
            vm.formData.Roles = [];
        };

        // Configuración de validación
        const validationSchema = {
            TipoDocumentoId: { required: true, message: "Seleccione el tipo de documento" },
            TipoTramiteId: { required: true, message: "Seleccione el trámite" },
            IdFase: { required: true, message: "Seleccione la fase" },
            Visible: { required: false, message: "Seleccione la validación de visibilidad" },
            Obligatorio: { required: false, message: "Indique si el campo es obligatorio" },
            Roles: { required: true, message: "Seleccione al menos un rol" },
            ValidacionId: { required: false, message: "" },
            ObligatoriedadId: { required: false, message: "" }
        };

        // Función para cerrar el modal
        vm.actionCerrar = function () {
            $uibModalInstance.close(false);
        };

        // Función para guardar los datos
        vm.actionGuardar = function () {            
            if (validar()) {
                if (!vm.dataEdit.TipoDocumento) {
                    modalusoServicio.crearDocumento(vm.formData).then((response) => {
                        const respuesta = JSON.parse(JSON.parse(response.data));
                        if (respuesta.Codigo == 1) {
                            utilidades.mensajeSuccess(respuesta.Mensaje, false, false, false);
                            $uibModalInstance.close(true);
                        }
                        else if (respuesta.Codigo != 1) utilidades.mensajeError(respuesta.Mensaje, false);
                    }).catch(error => {
                        utilidades.mensajeError("Error al guardar el documento", false);
                    });
                } else {
                    modalusoServicio.actualizarDocumento(vm.formData).then((response) => {
                        const respuesta = JSON.parse(JSON.parse(response.data));
                        if (respuesta.Codigo == 1) {
                            utilidades.mensajeSuccess(respuesta.Mensaje, false, false, false);
                            $uibModalInstance.close(true);
                        }
                        else if (respuesta.Codigo != 1) utilidades.mensajeError(respuesta.Mensaje, false);
                    }).catch(error => {
                        utilidades.mensajeError("Error al guardar el documento", false);
                    });
                }
                
            } else {
                console.log("Errores de validación:", vm.mensajeValidacion);
            }          
        };

        // Función de validación basada en el esquema de validación
        function validar() {
            vm.mensajeValidacion = {}; // Limpiar mensajes de validación
            let isValid = true;

            // Recorre el esquema de validación para comprobar cada campo            
            for (const field in validationSchema) {
                const rule = validationSchema[field];

                // Validar Roles como arreglo
                if (field === "Roles") {                    
                    if (rule.required && (!vm.formData.Roles || vm.formData.Roles.length === 0)) {
                        vm.mensajeValidacion[field] = rule.message;
                        isValid = false;
                    } else {
                        vm.mensajeValidacion[field] = null;
                    }
                }
                // Validar otros campos
                else if (rule.required && (!vm.formData[field] || vm.formData[field] === "")) {
                    vm.mensajeValidacion[field] = rule.message;
                    isValid = false;
                } else {
                    vm.mensajeValidacion[field] = null;
                }
            }            
            return isValid;
        }

        // Función para agregar un rol
        vm.addRole = function () {
            const selectedRoleId = vm.formData.RoleTemp; // Temporal para manejar la selección
            if (selectedRoleId && !vm.formData.Roles.find(role => role.IdRol === selectedRoleId)) {
                const roleObj = vm.Referencias.Roles.find(role => role.IdRol === selectedRoleId);
                if (roleObj) {
                    vm.formData.Roles.push(roleObj);
                    vm.formData.RoleTemp = ""; // Limpiar el select temporal
                }
            }
        };

        // Función para eliminar un rol
        vm.removeRole = function (roleId) {
            vm.formData.Roles = vm.formData.Roles.filter(role => role.IdRol !== roleId);            
        };

        vm.removeSelection = function (role) {
            role.selected = false;
            vm.formData.Roles = vm.formData.Roles.filter(r => r.IdRol !== role.IdRol);
        };

        /**
        * FUNCIONES DE FILTRADO DE DATOS
        * 
        **/

        // Fases debemos filtrarlos con los datos de Trámite 

        vm.filteredFases = []; // Array inicial para las fases filtradas
        

        vm.onTramiteChange = function (tramite) {
            vm.clearSelection()
            vm.filteredRoles = [];
            vm.filteredFases = [];
            if (tramite) {
                // Filtra las fases según el ID de trámite
                vm.filteredFases = vm.Referencias.Fases.filter(function (fase) {
                    return fase.TipoTramiteId === tramite; // Asegúrate de que Fases tenga la propiedad `TramiteId`
                });
            }
        };

        
        vm.onFaseChange = function (fase) {
            vm.clearSelection()
            vm.filteredRoles = [];
            if (fase) {
                vm.filteredRoles = vm.Referencias.Roles.filter(function (rol) {                    
                    return ((fase === rol.FaseId) && (rol.TipoTramiteId === vm.formData.TipoTramiteId));
                });
            }
        }

        // Inicialización del controlador
        vm.$onInit = function () {
            // Declaración de métodos
            vm.cerrar = this.cerrar;
            vm.guardar = this.guardar;            

            // Carga de datos para los selects
            vm.Referencias = $sessionStorage.Referencias;

            //Organizar los documentos por Orden Alfabetico
            if (vm.Referencias && vm.Referencias.Documentos) {
                vm.Referencias.Documentos.sort((a, b) => {                    
                    const descripcionA = a.Descripcion.toLowerCase();
                    const descripcionB = b.Descripcion.toLowerCase();

                    if (descripcionA < descripcionB) {
                        return -1;
                    } else if (descripcionA > descripcionB) {
                        return 1;
                    } else {
                        return 0;
                    }
                });
            }
            //Organizar los Trámites por Orden Alfabetico
            if (vm.Referencias && vm.Referencias.Tramites) {
                vm.Referencias.Tramites.sort((a, b) => {
                    const nombreA = a.Nombre.toLowerCase();
                    const nombreB = b.Nombre.toLowerCase();

                    if (nombreA < nombreB) {
                        return -1;
                    } else if (nombreA > nombreB) {
                        return 1;
                    } else {
                        return 0;
                    }
                });
            }

            //Organizar las Fases por Orden Alfabetico
            if (vm.Referencias && vm.Referencias.Fases) {
                vm.Referencias.Fases.sort((a, b) => {                    
                    const nombreA = a.NombreFase.toLowerCase();
                    const nombreB = b.NombreFase.toLowerCase();

                    if (nombreA < nombreB) {
                        return -1;
                    } else if (nombreA > nombreB) {
                        return 1;
                    } else {
                        return 0;
                    }
                });
            }
            //Organizar las Roles por Orden Alfabetico
            if (vm.Referencias && vm.Referencias.Roles) {
                vm.Referencias.Roles.sort((a, b) => {
                    const nombreA = a.NombreRol.toLowerCase();
                    const nombreB = b.NombreRol.toLowerCase();

                    if (nombreA < nombreB) {
                        return -1;
                    } else if (nombreA > nombreB) {
                        return 1;
                    } else {
                        return 0;
                    }
                });
            }            

            // Si se trata de una edición, cargar los datos existentes en formData
            
            if ($sessionStorage.modalEdit) {
                vm.dataEdit = $sessionStorage.dataEdit;
                const rolCorrespondiente = vm.Referencias.Roles.find(rol => rol.IdRol === vm.dataEdit.IdRol);
                rolCorrespondiente.selected = true
                vm.formData = {
                    Id: vm.dataEdit.Id,
                    TipoDocumentoId: vm.dataEdit.TipoDocumentoId,
                    TipoTramiteId: vm.dataEdit.TipoTramiteId,
                    Roles: [{
                        IdRol: rolCorrespondiente?.IdRol || null,
                        FaseId: vm.dataEdit.FaseId,
                        NombreRol: rolCorrespondiente?.NombreRol || null,
                        TipoTramiteId: vm.dataEdit.TipoTramiteId,
                        selected: rolCorrespondiente ? rolCorrespondiente.selected : false
                    }],
                    IdFase: vm.dataEdit.FaseId,
                    Obligatorio: vm.dataEdit.Obligatorio,
                    Visible: false,
                    ValidacionId: vm.dataEdit.ValidacionId,
                    ObligatoriedadId: vm.dataEdit.VisualizacionId,
                };                
            }
        };
    }
})();