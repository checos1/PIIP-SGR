(function () {
    'use strict';

    documentoController.$inject = [
        '$sessionStorage',
        '$scope',
        'documentoServicio',
        'utilidades',
        '$uibModal'
    ];

    function documentoController(
        $sessionStorage,
        $scope,
        documentoServicio,
        utilidades,
        $uibModal
    ) {

        const vm = this;

        // PESTAÑAS TABS

        vm.activarTabs = function (tab) {
            vm.tabs = tab;
            if (tab === 2) {
                vm.ConsultarReferencias();
                vm.actionConsultarUsoDocumento();
            }
        }

        /****
         * PESTAÑA CATÁLOGO TIPO DE DOCUMENTO
         *  Variables:
         *      - vm.formData: Objeto para almacenar todos los datos del formulario de creación / edición.
         *      - vm.formSearch: Objeto para almacenar el criterio de búsqueda por nombre.
         * 
         *  Funciones:
         * 
         *      - vm.actionResetForm: Reinicia los valores del formulario de registro.
         *      - vm.actionCancelar: Cancela la operación de creación / edición, oculta el formulario y lo reinicia.
         *      - vm.actionValidar: Valida los campos del formulario según el esquema de validación`validationSchema`.
         *      - vm.actionGuardar: Guarda o actualiza el documento según el valor de`formData.Id`.
         *      - vm.actionEdit: Carga los datos de un documento en`formData` para ser editado.
         *      - vm.actionAgregarElemento: Prepara el formulario para agregar un nuevo documento.
         *      - vm.actionEliminarDocumento: Elimina un documento tras confirmar la acción.
         *      - vm.actionEstadoDocumento: Cambia el estado activo / inactivo de un documento tras confirmar la acción.
         *      - vm.actionConsultarDocumento: Realiza la búsqueda de documentos según el criterio en`formSearch.Nombre`.
         */

        // Variables para almacenar todos los datos del formulario
        vm.formData = {
            Id: "",
            Nombre: "",
            Codigo: "",
            Activo: true,
        };


        // Variables para almacenar todos los datos del formulario
        vm.formSearch = {
            Nombre: "",
        };

        // Configuración de validación
        const validationSchema = {
            Nombre: { required: true, message: "Tipo Documento es un campo obligatorio" },
            Codigo: { required: false },
            Activo: { required: false }
        };

        //Resetear el formulario de registro
        vm.actionResetForm = function () {
            vm.formData.Id = "";
            vm.formData.Nombre = "";
            vm.formData.Codigo = "";
            vm.formData.Activo = true;

            vm.tieneMensajeValidacion = false;
            vm.mensajeValidacion = "";
        }

        //Cancelar 
        vm.actionCancelar = function () {
            // Iniciamos el Form
            vm.actionResetForm();

            //Ocultar el formulario
            vm.visibleNuevo = false;
        }

        vm.actionValidar = async function () {
            vm.mensajeValidacion = {}; // Limpiar mensajes de validación
            let isValid = true;
            // Recorre el esquema de validación para comprobar cada campo
            for (const field in validationSchema) {
                const rule = validationSchema[field];
                if (rule.required && (!vm.formData[field] || vm.formData[field] === "")) {
                    vm.mensajeValidacion[field] = rule.message;
                    isValid = false;
                } else {
                    vm.mensajeValidacion[field] = null;
                }
            }
            return isValid;
        }

        vm.actionGuardar = async function () {
            const isValid = await vm.actionValidar();
            if (isValid) {
                // Nuevo Registro                
                if (!vm.formData.Id) {

                    documentoServicio.crearDocumento(vm.formData.Nombre, vm.formData.Codigo, vm.formData.Activo).then(function (response) {
                        const respuesta = JSON.parse(JSON.parse(response.data));
                        if (respuesta.Codigo == 1) {
                            utilidades.mensajeSuccess(respuesta.Mensaje, false, false, false);
                            //Cargamos el registro
                            vm.actionConsultarDocumento(vm.formData.Nombre);

                            // Iniciamos el Form
                            vm.actionResetForm();

                            //Ocultar el formulario
                            vm.visibleNuevo = false;
                        }
                        else if (respuesta.Codigo != 1) utilidades.mensajeError(respuesta.Mensaje, false);
                    });
                } else {
                    documentoServicio.ActualizarDocumento(vm.formData.Id, vm.formData.Nombre, vm.formData.Codigo, vm.formData.Activo).then(function (response) {
                        const respuesta = JSON.parse(JSON.parse(response.data));
                        if (respuesta.Codigo == 1) {
                            utilidades.mensajeSuccess(respuesta.Mensaje, false, false, false);
                            //Cargamos el registro
                            vm.actionConsultarDocumento(vm.formData.Nombre);

                            // Iniciamos el Form
                            vm.actionResetForm();

                            //Ocultar el formulario
                            vm.visibleNuevo = false;
                        }
                        else if (respuesta.Codigo != 1) utilidades.mensajeError(respuesta.Mensaje, false);

                    });
                }
            }
        }

        vm.actionEdit = async function (item) {
            vm.visibleNuevo = true;
            vm.formData.Id = item.Id;
            vm.formData.Nombre = item.Descripcion;
            vm.formData.Codigo = item.Codigo;
            vm.formData.Activo = item.Activo;
        }

        vm.actionAgregarElemento = function () {
            vm.visibleNuevo = true;
            vm.actionResetForm();
            vm.formData.Nombre = vm.formSearch.Nombre;
            vm.lista = [];
        }

        vm.actionEliminarDocumento = function (Id) {
            //(mensaje, showCancelButton, funcionContinuar, funcionCancelar, titulo, confirmButtonText, cancelButtonText)
            utilidades.mensajeWarning("¿ Esta seguro de continuar ?",
                function funcionContinuar() {
                    return documentoServicio.EliminarDocumento(Id)
                        .then(function (response) {
                            const respuesta = JSON.parse(JSON.parse(response.data));
                            if (respuesta.Codigo == 1) {
                                utilidades.mensajeSuccess("", false, false, false, "El registro fue eliminado con éxito");
                                vm.actionConsultarDocumento('_blank_');
                                vm.formSearch.Nombre = ''
                            }
                            else if (respuesta.Codigo != 1) utilidades.mensajeError(respuesta.Mensaje, false, "");
                        });
                },
                function funcionCancelar() {

                }, "Aceptar", "Cancelar", "se va eliminar el registro.")

        }

        vm.actionEstadoDocumento = function (item, index) {

            utilidades.mensajeWarning("Esta seguro que desea cambiar el estado del registro ?",
                function funcionContinuar() {
                    return documentoServicio.EstadoDocumento(item)
                        .then(function (response) {
                            const respuesta = JSON.parse(JSON.parse(response.data));
                            if (respuesta.Codigo == 1) utilidades.mensajeSuccess(respuesta.Mensaje, false, false, false);
                            else if (respuesta.Codigo != 1) utilidades.mensajeError(respuesta.Mensaje, false);
                        })
                },
                function funcionCancelar() {
                    vm.pageData[index].Activo = !item.Activo;
                }, "Aceptar", "Cancelar", vm.mensajeWarning)

        }







        vm.actionConsultarDocumento = function (NumeroDocumento) {
            vm.visibleNuevo = false;
            vm.actionResetForm();
            vm.lista = [];
            if (NumeroDocumento === '') {
                vm.tieneMensajeValidacion = true;
                vm.mensajeValidacion = "Diligencie los campos señalados.";
                return;
            }

            documentoServicio.consultarDocumento(NumeroDocumento).then(function (response) {
                if (response.data) {
                    vm.lista = JSON.parse(JSON.parse(response.data));
                    vm.totalPages = Math.ceil(vm.lista.length / vm.pageSize);
                    vm.currentPage = 1; // Reiniciar la página actual
                    vm.updatePageData(); // Actualizar datos


                } else {
                    vm.visibleAgregar = true;
                    vm.lista = [];
                    vm.totalPages = 0;
                    vm.currentPage = 1;
                    vm.updatePageData();
                    utilidades.mensajeValidacion('El documento Consultado no existe');
                }
            });
        };


        /**
         * PESTANA CONFIGURAR USOS DE DOCUMENTO
         * 
         * 
         * 
         * */


        vm.formUsoData = {
            Id: "",
            TipoDocumentoId: "",
            TipoTramiteId: "",
            FaseId: "",
            RolId: "",
            ValidaObligatoriedadId: "",
            ValidaVisibilidadId: "",
            Obligatoriedad: true
        };

        vm.formUsoSearch = {
            TipoDocumentoId: "",
            TipoTramiteId: "",
            FaseId: "",
            RolId: "",
            ValidaObligatoriedad: "",
            ValidaVisibilidad: ""
        };

        vm.listaFiltradaUso = vm.listaUso = [];

        vm.actionConsultarUsoDocumento = function () {
            if (vm.listaUso.length === 0) {
                // Primera carga: llamar a ConsultarUsoDocumento para obtener los datos completos
                documentoServicio.ConsultarUsoDocumento().then(function (response) {
                    vm.listaUso = JSON.parse(JSON.parse(response.data));
                    vm.filtrarUsoDocumento();
                });
            } else {
                vm.filtrarUsoDocumento();
            }
        };

        // Función para filtrar los datos en el lado del cliente
        vm.filtrarUsoDocumento = function () {
            // Guardar el filtro aplicado en una variable para facilitar su acceso
            const filtro = vm.formUsoSearch;
            vm.listaFiltradaUso = [];
            // Filtrar los datos en vm.listaUso basado en los valores de filtro
            vm.listaFiltradaUso = vm.listaUso.filter(item => {                
                return (
                    (!filtro.TipoDocumentoId || item.TipoDocumentoId === filtro.TipoDocumentoId) &&
                    (!filtro.TipoTramiteId || item.TipoTramiteId === filtro.TipoTramiteId) &&
                    (!filtro.FaseId || item.FaseId === filtro.FaseId) &&
                    (!filtro.RolId || item.IdRol === filtro.RolId) &&
                    (!filtro.ValidaObligatoriedad || item.ValidacionId === filtro.ValidaObligatoriedad) &&
                    (!filtro.ValidaVisibilidad || item.VisualizacionId === filtro.ValidaVisibilidad)
                );
            });
        };


        vm.filteredSearchFases = [];
        vm.filteredSearchRoles = [];




        /** Filtro de Tramite para la busqueda */
        vm.onTramiteSearchChange = function (tramite) {
            vm.filteredSearchFases = [];
            vm.filteredSearchRoles = [];
            if (tramite) {
                // Filtra las fases según el ID de trámite
                vm.filteredSearchFases = vm.Referencias.Fases.filter(function (fase) {
                    return fase.TipoTramiteId === tramite;
                });
            }
        };

        vm.onFaseChange = function (faseId) {
            vm.filteredSearchRoles = [];
            if (faseId) {
                vm.filteredSearchRoles = vm.Referencias.Roles.filter(function (rol) {                    
                    return ((rol.FaseId === faseId) && (rol.TipoTramiteId === vm.formUsoSearch.TipoTramiteId))
                });
            }
        };


        /**
         * Filtro de la tabla principal
         * */






        // Configuración de la paginación
        vm.currentPage = 1; // Página actual
        vm.pageSize = 10; // Elementos por página
        vm.totalPages = 0; // Total de páginas (se calculará)
        vm.pageData = []; // Datos de la página actual
        vm.lista = []; // Datos completos para paginación
        vm.pages = []; // Páginas visibles en la paginación


        // Función para calcular los datos a mostrar en la página actual
        vm.updatePageData = function () {
            const start = (vm.currentPage - 1) * vm.pageSize;
            const end = start + vm.pageSize;
            vm.pageData = vm.lista.slice(start, end);
            vm.renderPagination();
        };

        // Cambiar página
        vm.changePage = function (page) {
            if (page >= 1 && page <= vm.totalPages) {
                vm.currentPage = page;
                vm.updatePageData();
            }
        };

        // Generar las páginas visibles para la paginación
        vm.renderPagination = function () {
            vm.pages = [];

            // Caso: Total de páginas <= 10 (mostrar todas las páginas)
            if (vm.totalPages <= 10) {
                for (let i = 1; i <= vm.totalPages; i++) {
                    vm.pages.push(i);
                }
                return; // Salimos de la función porque no necesitamos más lógica
            }

            // Siempre mostrar las primeras 3 páginas
            vm.pages.push(1, 2, 3);

            // Mostrar puntos suspensivos si hay un salto entre las primeras 3 páginas y las páginas centrales
            if (vm.currentPage > 5) {
                vm.pages.push('...');
            }

            // Mostrar las páginas cercanas a la actual
            const startPage = Math.max(4, vm.currentPage - 1);
            const endPage = Math.min(vm.totalPages - 3, vm.currentPage + 1);

            for (let i = startPage; i <= endPage; i++) {
                if (!vm.pages.includes(i)) {
                    vm.pages.push(i);
                }
            }

            // Mostrar puntos suspensivos si hay un salto entre las páginas centrales y las últimas 3 páginas
            if (vm.currentPage < vm.totalPages - 4) {
                vm.pages.push('...');
            }

            // Siempre mostrar las últimas 3 páginas
            vm.pages.push(vm.totalPages - 2, vm.totalPages - 1, vm.totalPages);
        };








        // Funciones de Carga de Datos
        /**
        vm.listTipoDocumento = function () {}
        vm.listTipoTramite = function () {}
        vm.listFase = function () {}
        vm.listRol = function () {}
        vm.listValidaVisibilidad = function () {}
        vm.listValidaObligatoriedad = function () { }
        */

        // Codigo Anterior Pendiente de Revisar 
        vm.init = function () {
            vm.Id = "";
            vm.tabs = 1;
            vm.visibleNuevo = false;
            vm.visibleAgregar = false;

            vm.Editar = false;
            vm.Agregar = false;
            vm.mensajeValidacion = null;
            vm.tieneMensajeValidacion = false;
            vm.mostrarFiltro = false;
            vm.mostrarFiltro2 = false;

            vm.Operacion = "1";


            vm.actionConsultarDocumento("_blank_");

        }
        vm.ConsultarReferencias = function () {
            documentoServicio.ReferenciasDocumento().then(function (response) {
                vm.Referencias = JSON.parse(JSON.parse(response.data));
                $sessionStorage.Referencias = vm.Referencias;
                //Organizar los Trámites por Orden Alfabetico
                               

                // Ordenar Tipos de Documento
                if (vm.Referencias.Documentos) {
                    vm.Referencias.Documentos.sort((a, b) => a.Descripcion.localeCompare(b.Descripcion));
                }

                // Ordenar Tipos de Trámite
                if (vm.Referencias.Tramites) {
                    vm.Referencias.Tramites.sort((a, b) => a.Nombre.localeCompare(b.Nombre));
                }

                // Ordenar Fases
                if (vm.Referencias.Fases) {
                    vm.Referencias.Fases.sort((a, b) => a.NombreFase.localeCompare(b.NombreFase));
                }

                // Ordenar Roles
                if (vm.Referencias.Roles) {
                    vm.Referencias.Roles.sort((a, b) => a.NombreRol.localeCompare(b.NombreRol));
                }
            });
        }




        vm.EditarUsoDocumento = function (item) {
            $sessionStorage.dataEdit = item;
            $sessionStorage.modalEdit = true;

            // Abrir modal para editar el uso de documento
            vm.abrirModal = function () {
                $uibModal.open({
                    templateUrl: 'src/app/administracion/documentos/modaluso/modaluso.html',
                    controller: 'modalusoController',
                    openedClass: "entidad-modal-adherencia",
                    controllerAs: 'vm',
                    
                }).result.then(function (data) {                    
                    if (data) {                        
                        vm.listaUso = [];
                        vm.actionConsultarUsoDocumento();
                    }
                });
            };

            
            vm.abrirModal();
        };

        vm.mostrarModalUso = function () {
            $sessionStorage.modalEdit = false;
            $uibModal.open({                
                templateUrl: 'src/app/administracion/documentos/modaluso/modaluso.html',
                controller: 'modalusoController',
                controllerAs: "vm",
                openedClass: "entidad-modal-adherencia",               
            }).result.then(function (data) {
                console.log("Data: ", data)
                if (data) {
                    vm.listaUso = [];
                    vm.actionConsultarUsoDocumento();
                }
            });
            
             
        }

        vm.actionEliminarUsoDocumento = function (item) {
            utilidades.mensajeWarning("¿ Esta seguro de continuar ?",
                function funcionContinuar() {
                    return documentoServicio.EliminarUsoDocumento(item.Id)
                        .then(function (response) {
                            const respuesta = JSON.parse(JSON.parse(response.data));
                            if (respuesta.Codigo == 1) {
                                utilidades.mensajeSuccess("", false, false, false, "El registro fue eliminado con éxito");                                
                                vm.listaUso = [];
                                vm.actionConsultarUsoDocumento();
                            }
                            else if (respuesta.Codigo != 1) utilidades.mensajeError(respuesta.Mensaje, false, "");
                        });
                },
                function funcionCancelar() {

                }, "Aceptar", "Cancelar", "se va eliminar el registro.")
        }

        vm.conmutadorFiltro = function () {
            vm.mostrarFiltro = !vm.mostrarFiltro;
            vm.formSearch.Nombre = "";
            let idSpanArrow = 'arrow-IdPanelBuscador';
            let arrowCapitulo = document.getElementById(idSpanArrow);
            let arrowClasses = arrowCapitulo.classList;
            if (!vm.mostrarFiltro) {
                vm.consultarDocumento('_blank_');
            }
            for (let i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
                    break;
                }
            }
        }
        vm.conmutadorFiltro2 = function () {
            vm.mostrarFiltro2 = !vm.mostrarFiltro2;
            let idSpanArrow = 'arrow-IdPanelBuscador';
            let arrowCapitulo = document.getElementById(idSpanArrow);
            let arrowClasses = arrowCapitulo.classList;
            for (let i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
                    break;
                }
            }
        }



        vm.limpiarCamposFiltro = async function () {
            vm.formSearch.Nombre = "";            
            let iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";            
            await vm.actionConsultarDocumento('_blank_');
        }

        vm.limpiarCamposUso = async function () {
            vm.formUsoSearch.TipoDocumentoId = "",
            vm.formUsoSearch.TipoTramiteId = "",
            vm.formUsoSearch.FaseId = "",
            vm.formUsoSearch.RolId = "",
            vm.formUsoSearch.ValidaObligatoriedad = "",
            vm.formUsoSearch.ValidaVisibilidad = ""            
            await vm.actionConsultarUsoDocumento();
        }
    }

    angular.module('backbone').controller(
        'documentoController',
        documentoController,
    );


})();
