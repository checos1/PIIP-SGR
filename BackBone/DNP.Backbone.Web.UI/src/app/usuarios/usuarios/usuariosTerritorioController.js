var usuarioCtrl = null;

(function () {
    'use strict';
    angular.module('backbone.usuarios').controller('usuariosTerritorioController', usuariosTerritorioController);

    usuariosTerritorioController.$inject = ['$scope', 'servicioUsuarios', 'servicioEntidades', 'constantesBackbone', 'constantesAutorizacion', 'utilidades', '$uibModal', '$window', 'FileSaver', '$location', 'Blob', 'backboneServicios'];

    function usuariosTerritorioController($scope, servicioUsuarios, servicioEntidades, constantesBackbone, constantesAutorizacion, utilidades, $uibModal, $window, FileSaver, $location, Blob, backboneServicios) {

        var vm = this;
        vm.consultarPermiso = backboneServicios.consultarPermiso;
        usuarioCtrl = vm;

        vm.init = init;

        vm.datos = [];

        vm.modelo = [];

        vm.entidadTerritorial = '';

        vm.tiposIdentificacion = [
            { id: 'CC', name: 'Cédula' },
            { id: 'NI', name: 'NIT' },
            { id: 'PA', name: 'Pasaporte' }
        ];

        /// Modals
        vm.abrirModalEditar = abrirModalEditar;
        vm.abrirModalPerfilesUsario = abrirModalPerfilesUsario;
        vm.abrirModalRolesXPerfil = abrirModalRolesXPerfil;
        vm.abrirModalEliminar = abrirModalEliminar;
        vm.abrirModalTipoInvitacionUsuario = abrirModalTipoInvitacionUsuario;
        vm.obtenerListadoPerfilesXEntidadBanco = obtenerListadoPerfilesXEntidadBanco;
        vm.obtenerListadoPerfilesXEntidad = obtenerListadoPerfilesXEntidad;
        vm.obtenerListadoPerfilesXEntidadYUsuario = obtenerListadoPerfilesXEntidadYUsuario;
        vm.obtenerListadoPerfilesXUsuarioTerritorio = obtenerListadoPerfilesXUsuarioTerritorio;
        vm.obtenerPerfilesBackBone = obtenerPerfilesBackBone;
        vm.abrirModalInvitarUsuarioDNP = abrirModalInvitarUsuarioDNP;
        vm.abrirModalInvitarUsuarioExterno = abrirModalInvitarUsuarioExterno;
        vm.accederPantallaPerfilesUsuario = accederPantallaPerfilesUsuario;
        vm.cambiarEstadoUsuario = cambiarEstadoUsuario;
        vm.registrarUsuarioPIIP = registrarUsuarioPIIP;
        vm.registrarUsuarioTerritorioPIIP = registrarUsuarioTerritorioPIIP;
        vm.registrarUsuarioAPPSTS = registrarUsuarioAPPSTS;
        vm.registrarUsuarioSTS = registrarUsuarioSTS;
        vm.enviarCorreoInvitacion = enviarCorreoInvitacion;
        vm.restablecerBusqueda = restablecerBusqueda;

        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;

        /// Filtros
        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.filtrar = filtrar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.filtro = null;
        vm.cantidadDeUsuarios = 0;
        vm.BusquedaRealizada = false;

        vm.filtros = {
            nombreUsuario: null,
            cuentaUsuario: null,
            estado: true,
            idPerfilBackbone: null,
            numeroDocumento: null,
            idEntidad: null,
            catalogos: {

                entidades: [],
                usuarioLista: []
            }
        };

        vm.uiSelect = {
            selected: null
        };

        /// Plantilla de acciones de la tabla
        vm.plantillaAccionesTabla = 'src/app/usuarios/usuarios/plantillas/plantillaAccionesTabla.html';
        vm.plantillaEstadoTabla = 'src/app/usuarios/usuarios/plantillas/plantillaEstadoTabla.html';

        /// Definiciones de componente
        vm.opciones = {
            cambiarTipoEntidadFiltro: cambiarTipoEntidadFiltro,
            nivelJerarquico: 1,
            BanderaTabla: 1,
            gridOptions: {
                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                paginationPageSize: 10,
            }
        };

        /// Definiciones de columna componente
        vm.columnDefs = [{
            field: 'Nombre',
            displayName: 'Nombre',
            enableHiding: false,
            width: '35%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'IdUsuarioDNP',
            displayName: 'Id Usuario DNPPP',
            enableHiding: false,
            width: '30%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'Estado',
            displayName: 'Estado',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaEstadoTabla,
            width: '15%'
        },
        {
            field: 'Accion',
            displayName: 'Acción',
            headerCellClass: 'text-center',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaAccionesTabla,
            width: '15%'
        }];

        function restablecerBusqueda() {
            vm.limpiarCamposFiltro();
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }

        function downloadExcel() {
            vm.filtro =
            { // Parametros filtro
                NombreUsuario: vm.filtros.nombreUsuario,
                CuentaUsuario: vm.filtros.cuentaUsuario,
                IdPerfilBackbone: vm.filtros.idPerfilBackbone,
                NumeroDocumento: vm.filtros.numeroDocumento,
                idEntidad: vm.filtros.idEntidad,
                Estado: null
            }
            servicioUsuarios.obtenerExcel(vm.tipoEntidad, vm.filtro).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, "Usuarios.xls");
            }, function (error) {
                utilidades.mensajeError("Error al realizar la operación.", false);

            });

        }

        function downloadPdf() {

            vm.filtro =
            { // Parametros filtro
                NombreUsuario: vm.filtros.nombreUsuario,
                CuentaUsuario: vm.filtros.cuentaUsuario,
                IdPerfilBackbone: vm.filtros.idPerfilBackbone,
                NumeroDocumento: vm.filtros.numeroDocumento,
                idEntidad: vm.filtros.idEntidad,
                Estado: null
            }
            servicioUsuarios.obtenerUsuariosPorEntidadPdf(vm.tipoEntidad, vm.filtro).then(
                function (data) {

                    servicioUsuarios.imprimirPdf(data.data).then(function (retorno) {

                        var blob = new Blob([retorno.data], {
                            type: "application/octet-stream"
                        });

                        FileSaver.saveAs(retorno.data, nombreDelArchivo(retorno));
                    });

                }, function (error) {
                    vm.Mensaje = error.data.Message;
                    utilidades.mensajeError("Error al realizar la operación." + vm.Mensaje, false);
                });
        };


        function nombreDelArchivo(response) {
            var filename = "";
            var disposition = response.headers("content-disposition");
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }
            return filename;
        }

        /**
         * 
         * @description Obtiene la lista de entidades filtradas por el tipo de entidad actual en la tab(pestaña)
         * @param {String} tipoEntidad . Tipo de entidad actual. Nacional, Territorial, etc.
         */
        vm.obtenerEntidadesPorTipo = function () {
            try {
                servicioEntidades.obtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(vm.tipoEntidad).then(response => {
                    var dataResponse = response.data;

                    vm.filtros.catalogos.entidades = dataResponse;
                });
            }
            catch (exception) {
                console.log('usuariosController.obtenerEntidadesPorTipo => ', exception.message);
                toastr.error('Ocurrió un error al cargar la lista de entidades');
            }
        };

        vm.AbrilNivel = function (IdEntidad) {
            vm.datos.forEach(function (value, index) {
                if (value.IdEntidad == IdEntidad) {
                    if (value.estadoEntidad == '+')
                        value.estadoEntidad = '-';
                    else
                        value.estadoEntidad = '+';
                }
            });
        }

        /// Comienzo
        function init() {

            try {
                vm.tipoEntidad = 'Territorial';
                vm.obtenerEntidadesPorTipo();
                vm.obtenerPerfilesBackBone();

                cambiarTipoEntidadFiltro(vm.tipoEntidad)
            }
            catch (exception) {
                console.log('usuariosController.init => ', exception.message);
                toastr.error('Ocurrió un error al cargar los catálogos');
            }
        }

        servicioEntidades.registrarObservador(function (datos) {
            if (typeof datos !== 'undefined') {
                if (typeof datos.modelo !== 'undefined') {
                    vm.modelo = datos.modelo;
                }
            }
        });

        function conmutadorFiltro() {
            limpiarCamposFiltro2();
            vm.mostrarFiltro = !vm.mostrarFiltro;
            var idSpanArrow = 'arrow-IdPanelBuscador';
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
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

        function filtrar() {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
            listarUsuarios();
            vm.BusquedaRealizada = true;
        }

        function limpiarCamposFiltro() {
            vm.filtro = null;
            vm.filtros.nombreUsuario = null;
            vm.filtros.cuentaUsuario = null;
            vm.filtros.estado = true;
            vm.filtros.numeroDocumento = null;
            vm.filtros.idPerfilBackbone = null;
            vm.BusquedaRealizada = false;
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
            listarUsuarios();

        }
        function limpiarCamposFiltro2() {
            vm.filtro = null;
            vm.filtros.nombreUsuario = null;
            vm.filtros.cuentaUsuario = null;
            vm.filtros.estado = true;
            vm.filtros.idPerfilBackbone = null;
            vm.filtros.numeroDocumento = null;
            vm.BusquedaRealizada = false;
            vm.filtros.numeroDocumento = null;
        }

        /// Getters
        function listarUsuarios() {
            vm.usuariosFiltro = [];
            vm.datos = null;
            servicioUsuarios.obtenerUsuariosXEntidadTerritorio(
                vm.tipoEntidad,
                vm.filtro,
                { // Parametros filtro
                    NombreUsuario: vm.filtros.nombreUsuario,
                    CuentaUsuario: vm.filtros.cuentaUsuario,
                    IdPerfilBackbone: vm.filtros.idPerfilBackbone,
                    NumeroDocumento: vm.filtros.numeroDocumento,
                    Estado: null
                }
            )
                .then(function (response) {
                    var datos = response.data;
                    vm.cantidadDeUsuarios = 0;

                    if (datos != null && datos.length > 0) {

                        if (vm.filtros.idEntidad != null) {
                            datos = datos.filter(x => x.IdEntidad == vm.filtros.idEntidad);
                        }

                        datos.forEach(item => {
                            item.agrupadorEntidad = item.AgrupadorEntidad;
                            item.entidad = item.Entidad;
                            item.tipoEntidad = item.TipoEntidad;
                            item.estadoEntidad = "+";
                            vm.cantidadDeUsuarios += item.Usuarios.length;
                            item.Usuarios.forEach(usuario => {
                                usuario.idEntidad = item.IdEntidad;
                                usuario.nombreEntidad = item.agrupadorEntidad + ' - ' + item.entidad;
                                usuario.IdEntidad = item.IdEntidad;
                            });
                            item.subGridOptions = {
                                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                paginationPageSize: 5,
                                data: item.Usuarios
                            }
                        });
                    }

                    vm.entidadTerritorial = datos.find(x => x.TipoEntidad == 'Territorial' && (x.Nivel == 0 || (x.Nivel == 1 && x.AgrupadorEntidad == ''))).Entidad;
                    vm.modelo[0].Nombre = vm.entidadTerritorial;
                    vm.datos = datos.filter(x => x.subGridOptions.data.length > 0 && (x.TipoEntidad == 'UnidadResponsable' && x.Nivel === 0) || (x.TipoEntidad == 'Territorial' && x.Nivel === 2))
                });
        }

        // Actions
        function cambiarTipoEntidadFiltro(tipoEntidad) {
            vm.tipoEntidad = tipoEntidad;

            vm.uiSelect.selected = null;
            vm.obtenerEntidadesPorTipo();
            listarUsuarios();
        }

        function abrirModalEditar(obj) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalAccionUsuario.html',
                controller: 'modalAccionUsuarioController',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    obj: obj,
                }
            }).result.then(function (result) {
                listarUsuarios();
            }, function (reason) {
                listarUsuarios();
            });
        }

        function abrirModalPerfilesUsario(obj) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalPerfilesUsuarioTerritorio.html',
                controller: 'modalPerfilesUsuarioTerritorioController',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    obj: obj,
                },
                actions: {
                    registrarUsuarioPIIP: vm.registrarUsuarioPIIP,
                    obtenerListadoPerfilesXEntidadBanco: vm.obtenerListadoPerfilesXEntidadBanco,
                    obtenerListadoPerfilesXEntidad: vm.obtenerListadoPerfilesXEntidad,
                    obtenerListadoPerfilesXUsuario: vm.obtenerListadoPerfilesXUsuario,
                    abrirModalInvitarUsuarioDNP: vm.abrirModalInvitarUsuarioDNP,
                    abrirModalInvitarUsuarioExterno: vm.abrirModalInvitarUsuarioExterno
                }
            }).result.then(function (result) {
                //listarUsuarios();
            }, function (reason) {
                //listarUsuarios();
            });
        }

        function abrirModalRolesXPerfil(obj) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalRolesXPerfil.html',
                controller: 'modalRolesXPerfilController',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    obj: obj,
                },
                actions: {
                    registrarUsuarioPIIP: vm.registrarUsuarioPIIP,
                    obtenerListadoPerfilesXEntidad: vm.obtenerListadoPerfilesXEntidad,
                    obtenerListadoPerfilesXUsuario: vm.obtenerListadoPerfilesXUsuario,
                    abrirModalInvitarUsuarioDNP: vm.abrirModalInvitarUsuarioDNP,
                    abrirModalInvitarUsuarioExterno: vm.abrirModalInvitarUsuarioExterno
                }
            }).result.then(function (result) {
                //listarUsuarios();
            }, function (reason) {
                // listarUsuarios();
            });
        }

        function abrirModalEliminar(obj) {
            utilidades.mensajeWarning("Confirma la exclusión del registro?", function funcionContinuar() {
                servicioUsuarios.eliminarUsuarioPerfil(obj.IdUsuarioPerfil, obj.IdEntidad, obj.IdUsuarioDNP)
                    .then(function (response) {
                        if (response.data.Exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            listarUsuarios();
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }

                    }, function (response) {
                        if (response.status == 409) {
                            utilidades.mensajeError("No es posible eliminar este perfil de usuario, ya que tiene proyectos vinculados.", false);
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }
                    })
            }, function funcionCancelar() {
            })
        }

        function accederPantallaPerfilesUsuario(usuario) {
            $location.path(`/usuarios/${usuario.Id}`);
        }

        function cambiarEstadoUsuario(obj) {
            var mensaje1 = null;

            if (usuarioDNP == obj.IdUsuarioDNP) {

                utilidades.mensajeError("No es posible cambiar el estado de un usuario de un perfil que no administra.");
                obj.Activo = !obj.Activo;
            }
            else {

                if (!obj.Activo) {
                    var mensaje1 = "El usuario será inactivado de la entidad.";
                }
                else {
                    mensaje1 = "El usuario será activado en la entidad.";
                }

                utilidades.mensajeWarning("Si este está registrado únicamente en esta entidad. No podrá acceder a la PIIP hasta que el administrador reactive su estado. Si este está registrado está en otras entidades, se le ocultarán los espacios y las operaciones relacionados con esta entidad, hasta que el administrador reactive su estado. ¿Está seguro de continuar ?",
                    function funcionContinuar() {

                        servicioUsuarios.validarPermisoInactivarUsuario(usuarioDNP, obj.IdUsuarioDNP).then(function (responsex) {
                            if (responsex.data == "OK") {
                                //function setActivoUsuarioPerfil(idUsuarioPerfil, activo, idUsuarioDnp, idEntidad) {
                                //servicioUsuarios.setActivoUsuarioPerfilPorEntidad(obj.Id, obj.IdEntidad, obj.Activo, obj.IdUsuarioDNP, vm.tipoEntidad)
                                servicioUsuarios.setActivoUsuarioEntidad(obj.Id, obj.IdEntidad, obj.Activo, obj.IdUsuarioDNP, vm.tipoEntidad)
                                    .then(function (response) {
                                        if (response.data) {
                                            utilidades.mensajeSuccess(mensaje1, false, false, false);
                                        }
                                        else {
                                            utilidades.mensajeError("Error al realizar la operación", false);
                                            obj.Activo = !obj.Activo;
                                        }
                                    });
                            }
                            else {
                                utilidades.mensajeError("No es posible cambiar el estado de un usuario de un perfil que no administra.");
                                obj.Activo = !obj.Activo;
                            }

                        }, function (error) {
                            console.log("error", error);
                        });
                    },
                    function funcionCancelar() {
                        obj.Activo = !obj.Activo;
                    }, null, null, mensaje1);
            }
        }

        function obtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario) {
            var promise = servicioUsuarios.obtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario)
            return promise;
        }

        function obtenerListadoPerfilesXUsuarioTerritorio(idUsuario) {
            var promise = servicioUsuarios.obtenerListadoPerfilesXUsuarioTerritorio(idUsuario)
            return promise;
        }

        function obtenerListadoPerfilesXEntidadBanco(idEntidad) {
            var promise = servicioEntidades.obtenerListadoPerfilesXEntidadBanco(idEntidad)
            return promise;
        }

        function obtenerListadoPerfilesXEntidad(idEntidad) {
            var promise = servicioEntidades.obtenerListadoPerfilesXEntidad(idEntidad)
            return promise;
        }
        function obtenerPerfilesBackBone() {
            var parametros = {
                usuario: usuarioDNP,
                aplicacion: 'backbone',
            }
            servicioUsuarios.obtenerPerfilesAutorizadosPorAplicacion(parametros)
                .then(function (response) {
                    vm.listaPerfilesBackbone = response.data;
                }, function (error) {
                    console.log("error", error);
                });
        }

        function abrirModalInvitarUsuarioDNP() {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalInvitarUsuarioDNPTemplate.html',
                controller: 'modalInvitarUsuarioDNPController',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    objInvitarUsuario: {
                        tipoEntidad: vm.tipoEntidad,
                        tiposIdentificacion: vm.tiposIdentificacion,
                        listaEntidades: vm.listadoEntidades,
                        listaPerfilesBackbone: []
                    },
                    actions: {
                        registrarUsuarioPIIP: vm.registrarUsuarioPIIP,
                        obtenerListadoPerfilesXEntidad: vm.obtenerListadoPerfilesXEntidad,
                        obtenerListadoPerfilesXEntidadYUsuario: vm.obtenerListadoPerfilesXEntidadYUsuario,
                        abrirModalInvitarUsuarioDNP: vm.abrirModalInvitarUsuarioDNP,
                        abrirModalInvitarUsuarioExterno: vm.abrirModalInvitarUsuarioExterno
                    }
                }
            }).result.then(function (result) {

            }, function (reason) {

            });
        }

        function abrirModalInvitarUsuarioExterno() {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalInvitarUsuarioExternoTerritorioTemplate.html',
                controller: 'modalInvitarUsuarioExternoTerritorioController',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    objInvitarUsuario: {
                        tipoEntidad: vm.tipoEntidad,
                        tiposIdentificacion: vm.tiposIdentificacion,
                        modelo: vm.modelo,
                        listaPerfilesBackbone: []
                    },
                    actions: {
                        registrarUsuarioPIIP: vm.registrarUsuarioPIIP,
                        registrarUsuarioTerritorioPIIP: vm.registrarUsuarioTerritorioPIIP,
                        registrarUsuarioAPPSTS: vm.registrarUsuarioAPPSTS,
                        registrarUsuarioSTS: vm.registrarUsuarioSTS,
                        obtenerListadoPerfilesXEntidad: vm.obtenerListadoPerfilesXEntidad,
                        obtenerListadoPerfilesXEntitadYUsuario: vm.obtenerListadoPerfilesXEntidadYUsuario,
                        obtenerListadoPerfilesXUsuarioTerritorio: vm.obtenerListadoPerfilesXUsuarioTerritorio,
                        abrirModalInvitarUsuarioDNP: vm.abrirModalInvitarUsuarioDNP,
                        abrirModalInvitarUsuarioExterno: vm.abrirModalInvitarUsuarioExterno
                    }
                }
            }).result.then(function (result) {
                listarUsuarios();
            }, function (reason) {
                listarUsuarios();
            });
        }

        function abrirModalTipoInvitacionUsuario() {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalTipoInvitacionUsuarioTemplate.html',
                controller: 'modalTipoInvitacionUsuarioController',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    objInvitarUsuario: {
                        tipoEntidad: vm.tipoEntidad
                    },
                    actions: {
                        abrirModalInvitarUsuarioDNP: vm.abrirModalInvitarUsuarioDNP,
                        abrirModalInvitarUsuarioExterno: vm.abrirModalInvitarUsuarioExterno
                    }
                }
            }).result.then(function (result) {

            }, function (reason) {

            });
        }

        function enviarCorreoInvitacion(modelo) {

            if (modelo.pPlantilla != '') {
                servicioUsuarios.enviarCorreoInvitacionSTS(modelo)
                    .then(function (response) {
                        let exito = response.data;
                        console.log('enviarCorreoInvitacionSTS response.data ', response.data)
                        utilidades.mensajeSuccess("El usuario recibirá un mensaje por correo electrónico confirmándole las novedades de sus perfiles en la PIIP.", false, false, false, "La invitación fue enviada con éxito!");
                        console.log('enviar correo')
                    })
                    .catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }

                        utilidades.mensajeError("Error al realizar la operación");
                    });
            }

        }

        function registrarUsuarioPIIP(modelo) {

            servicioUsuarios.registrarUsuarioPIIP(modelo)
                .then(function (response) {
                    let exito = response.data;
                    if (utilidades.isTrue(exito)) {
                        let prepararCorreo = {}
                        if (modelo.correo.includes("@dnp.gov.co")) {
                            prepararCorreo = {
                                pAplicacion: constantesBackbone.keySTSApplication,
                                pTipoDocumento: modelo.tipoIdentificacion,
                                pNumeroDocumento: modelo.identificacion,
                                pAsunto: "Invitación usuarios PIIP",
                                pEntidad: modelo.nombreEntidad,
                                pPerfil: modelo.nombrePerfil,
                                pCorreo: modelo.correo,
                                pUsuario: modelo.nombre + " " + modelo.apellido,
                                pPassword: "No aplica",
                                pPlantilla: "usuarioDNP"
                            }

                            vm.enviarCorreoInvitacion(prepararCorreo);

                        } else {
                            prepararCorreo = {
                                pAplicacion: constantesBackbone.keySTSApplication,
                                pTipoDocumento: modelo.tipoIdentificacion,
                                pNumeroDocumento: modelo.identificacion,
                                pAsunto: "Invitación usuarios PIIP",
                                pEntidad: modelo.nombreEntidad,
                                pPerfil: modelo.nombrePerfil,
                                pCorreo: modelo.correo,
                                pUsuario: modelo.nombre + " " + modelo.apellido,
                                pPassword: modelo.tipoInvitacion == 1 ? "No aplica" : utilidades.generatePasswordRand(8, 'rand'),
                                pPlantilla: modelo.tipoInvitacion == 1 ? "usuarioExternoExistenteAppConfiable" : "usuarioExternoExistenteAppNoConfiable"
                            }
                            if (modelo.tipoInvitacion) {
                                vm.enviarCorreoInvitacion(prepararCorreo);
                            }
                        }

                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }

                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function registrarUsuarioTerritorioPIIP(modelo, existeUsuarioPIIP) {
            servicioUsuarios.registrarUsuarioTerritorioPIIP(modelo).then(function (response) {
                let exito = response.data;
                if (utilidades.isTrue(exito)) {
                    if (!existeUsuarioPIIP) {
                        let prepararCorreo = {}
                        if (modelo.correo.includes("@dnp.gov.co")) {
                            prepararCorreo = {
                                pAplicacion: constantesBackbone.keySTSApplication,
                                pTipoDocumento: modelo.tipoIdentificacion,
                                pNumeroDocumento: modelo.identificacion,
                                pAsunto: "Invitación usuarios PIIP",
                                pEntidad: modelo.nombreEntidad,
                                pPerfil: modelo.nombrePerfil,
                                pCorreo: modelo.correo,
                                pUsuario: modelo.nombre + " " + modelo.apellido,
                                pPassword: "No aplica",
                                pPlantilla: "usuarioDNP"
                            }
                            vm.enviarCorreoInvitacion(prepararCorreo);
                        } else {
                            prepararCorreo = {
                                pAplicacion: constantesBackbone.keySTSApplication,
                                pTipoDocumento: modelo.tipoIdentificacion,
                                pNumeroDocumento: modelo.identificacion,
                                pAsunto: "Invitación usuarios PIIP",
                                pEntidad: modelo.nombreEntidad,
                                pPerfil: modelo.nombrePerfil,
                                pCorreo: modelo.correo,
                                pUsuario: modelo.nombre + " " + modelo.apellido,
                                pPassword: modelo.tipoInvitacion == 1 ? "No aplica" : utilidades.generatePasswordRand(8, 'rand'),
                                pPlantilla: modelo.tipoInvitacion == 1 ? "usuarioExternoExistenteAppConfiable" : "usuarioExternoExistenteAppNoConfiable"
                            }
                            if (modelo.tipoInvitacion) {
                                vm.enviarCorreoInvitacion(prepararCorreo);
                            }
                        }
                    } else {
                        utilidades.mensajeSuccess("Se ha actualizado la información de perfiles del usuario en la PIIP.", false, false, false, "Información actualizda con éxito!");
                    }
                }
                else {
                    utilidades.mensajeError("Error al realizar la operación", false);
                }
            })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function registrarUsuarioAPPSTS(modelo) {

            servicioUsuarios.registrarUsuarioAPPSTS(modelo)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                        let prepararCorreo = {
                            pAplicacion: constantesBackbone.keySTSApplication,
                            pTipoDocumento: modelo.pTipoDocumento,
                            pNumeroDocumento: modelo.pNumeroDocumento,
                            pAsunto: "Invitación usuarios PIIP",
                            pEntidad: modelo.nombreEntidad,
                            pPerfil: modelo.nombrePerfil,
                            pCorreo: modelo.pCorreo,
                            pUsuario: modelo.pUsuario,
                            pPassword: modelo.pPassword,
                            pPlantilla: modelo.pPlantilla

                        }

                        if (!prepararCorreo.pCorreo.includes("@dnp.gov.co") && !prepararCorreo.pAplicacion.includes("MGA")) {
                            vm.enviarCorreoInvitacion(prepararCorreo);
                        }

                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }

                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function registrarUsuarioSTS(modelo) {

            servicioUsuarios.registrarUsuarioSTS(modelo)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                        let prepararCorreo = {
                            pAplicacion: constantesBackbone.keySTSApplication,
                            pTipoDocumento: modelo.pTipoDocumento,
                            pNumeroDocumento: modelo.pNumeroDocumento,
                            pAsunto: "Invitación usuarios PIIP",
                            pEntidad: modelo.nombreEntidad,
                            pPerfil: modelo.nombrePerfil,
                            pCorreo: modelo.pCorreo,
                            pUsuario: modelo.pUsuario,
                            pPassword: modelo.pPassword,
                            pPlantilla: modelo.pPlantilla
                        }

                        if (!prepararCorreo.pCorreo.includes("@dnp.gov.co") && !prepararCorreo.pAplicacion.includes("MGA")) {
                            vm.enviarCorreoInvitacion(prepararCorreo);
                        }

                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    console.log('error: ', error)
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.usuarios')
        .component('usuariosTerritorio', {
            templateUrl: "/src/app/usuarios/usuarios/usuariosTerritorio.html",
            controller: 'usuariosTerritorioController',
            controllerAs: 'vm',
        });
})();