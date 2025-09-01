(function () {
    'use strict';

    detalleProyectosUnTipoFormularioSgp.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'flujoServicios',
        '$scope',
        'trasladosServicio',
        'justificacionCambiosServicio',
        'constantesBackbone',
        'modalActualizaEstadoAjusteProyectoServicio',
        '$location',
        'comunesServicio'
    ];

    function detalleProyectosUnTipoFormularioSgp(
        $sessionStorage,
        sesionServicios,
        utilidades,
        flujoServicios,
        $scope,
        trasladosServicio,
        justificacionCambiosServicio,
        constantesBackbone,
        modalActualizaEstadoAjusteProyectoServicio,
        $location,
        comunesServicio
    ) {

        var vm = this;
        vm.lang = "es";
        //vm.estadoAlertaTemplate = 'src/app/panelPrincial/modales/tramite/plantillaSeleccionar.html';
        $sessionStorage.existecontradistribucion = false;
        vm.IdEntidad = $sessionStorage.idEntidad;
        vm.eliminarAsociacion = eliminarAsociacion;
        vm.actualizaEstado = actualizaEstado;
        vm.aceptar = aceptar;
        vm.cancelar = cancelar;
        vm.seleccionarTodos = seleccionarTodos;
        vm.devolverSeleccion = devolverSeleccion;
        vm.BPIN = '';
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;
        vm.tituloMensaje = "";
        vm.mensaje = "";
        vm.textoBuscar = '';
        vm.checkSeleccionar = false;
        vm.estadoTramite = '';
        vm.estadoAjusteCreado = false;
        vm.proyectoAsociado = false;
        vm.proyectoId = 0;
        vm.datosproyecto = {};
        vm.listavalores = [];
        $sessionStorage.EstadoAsociacionVF = '';
        $sessionStorage.EstadoAjusteCreado = false;
        $sessionStorage.EstadoDNpAplicado = true;
        vm.nombreEstadoAsociacion = 'Con proyectos asociados';
        vm.estadoAsociacion = true;
        vm.cantProyectosAsociados = 0;
        vm.visibleValidar = true;
        vm.Editar = "EDITAR";
        vm.disabled = true;
        vm.datosproyecto = {};
        vm.BPINesParaAjustarValores = [];
        vm.listaValoresAComparar = [];
        vm.envioSolicitudPendiente = $sessionStorage.envioSolicitudPendiente === undefined ? false : $sessionStorage.envioSolicitudPendiente;
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes($sessionStorage.listadoAccionesTramite[0].Nombre) && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.origenrecursos = 0;
        vm.rubrofuncionamiento = "";
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.changeVerifica = changeVerifica;
        //Para mostar los botones en el paso 3


        //Valida que este en el paso 3 para mostrar los botones
        vm.mostrarPaso3 = $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelElaboracionConcepto;
        vm.mostrarPaso1 = $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelSeleccionProyectos;
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;


        vm.tituloMensaje = "";
        vm.mensaje = ""

        //Verifica que el concepto esta pendiente deshabilita todos lo botones del paso 3
        //$scope.$watch('vm.deshabilitar', function () {
        //    if (vm.deshabilitar === "true") {
        //        !vm.habilitaBotonesPaso3 && vm.rolanalista.toLowerCase() === 'true' && $sessionStorage.nombreAccion.includes('Elaboración concepto') && !vm.envioSolicitudPendiente && !$sessionStorage.soloLectura ? true : false;
        //    }
        //    else if (vm.deshabilitar === "false") {
        //        vm.habilitaBotonesPaso3 && vm.rolanalista.toLowerCase() === 'true' && $sessionStorage.nombreAccion.includes('Elaboración concepto') && !vm.envioSolicitudPendiente && !$sessionStorage.soloLectura ? true : false;
        //    }

        //});

        vm.peticionObtenerInbox = {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoTramite,
            // ReSharper disable once UndeclaredGlobalVariableUsing
            Aplicacion: nombreAplicacionBackbone,
            IdInstancia: $sessionStorage.idInstanciaIframe,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdsEtapas: getIdEtapa()
        };
        $scope.$watch('vm.actualizacomponentes', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                ObtenerProyectosTramite();
                getOrigenRecursosTramite();
                vm.disabled = true;
                vm.unSoloTipoOperacion = vm.unsolotipooperacion == "true" ? true : false;
                if (vm.tipotramiteid == "9") {//Adicion
                    vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Asociar Proyectos') && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
                    $sessionStorage.actualizadetalle = vm.actualizadetalle;
                }
            }
        });
        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            }
        });

        vm.init = function () {
            if ($sessionStorage.usuario.roles.find(x => x.Nombre.includes('Analista')))
                vm.rolAnalista3 = true;
            else
                vm.rolAnalista3 = false;
            //$scope.$watch(() => vm.tramiteid
            //    , (newVal, oldVal) => {
            //        if (newVal) {
            //            if (vm.tramiteid !== undefined && vm.tramiteid !== '') {
            //                ObtenerProyectosTramite();
            //                vm.disabled = true;
            //                if (vm.tipotramiteid==7)//incorporaciones
            //                    vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Creación del trámite') && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
            //            }
            //        }
            //    }, true);

            $scope.$watch(() => vm.actualizadetalle
                , (newVal, oldVal) => {
                    if (newVal) {
                        if (vm.tramiteid !== undefined && vm.tramiteid !== '')
                            ObtenerProyectosTramite();
                        getOrigenRecursosTramite();
                        vm.disabled = true;
                        vm.unSoloTipoOperacion = vm.unsolotipooperacion == "true" ? true : false;
                        if (vm.tipotramiteid == "9") {//Adicion
                            vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Asociar Proyectos') && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
                            $sessionStorage.actualizadetalle = vm.actualizadetalle;
                        }
                    }
                }, true);

            $scope.$watch('vm.rolanalista', function () {
                if (vm.rolanalista !== '' && vm.rolanalista !== undefined && vm.rolanalista !== null) {
                    vm.rolAnalista3 = vm.rolanalista.toLowerCase() === 'true' ? true : false;// && $sessionStorage.nombreAccion.includes('Elaboración concepto') && !vm.envioSolicitudPendiente && !$sessionStorage.soloLectura ? true : false;
                }
            });
        }



        function ObtenerProyectosTramite() {
            vm.listaProyectosConsultados = [];
            vm.listaEntidadesProy = [];
            vm.listaGrupoEntidades = [];
            vm.listaGrupoProyectos = [];
            vm.datoproyectosTramite = [];
            vm.listaproyectosEntidad = [];
            vm.listaEntidadesGrilla = [];
            vm.listaGrillaProyectos = [];
            //vm.gridOptions.data = vm.listaGrillaProyectos;
            // vm.gridOptions.columnDefs = [];

            vm.ValorTotalMontoRC = 0;
            vm.ValorTotalMontoPropiosRC = 0;
            vm.ValorTotalMontoNacionRC = 0;
            vm.ValorTotalMontoRCC = 0;
            vm.ValorTotalMontoNacionRCC = 0;
            vm.ValorTotalMontoPropiosRCC = 0;

            comunesServicio.obtenerProyectosTramite(vm.tramiteid)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {

                        vm.listaProyectosConsultados = response.data;
                        vm.listaEntidadesProy = [];
                        if (vm.tipoproyecto == undefined)
                            vm.listaEntidadesProy = response.data;
                        else {
                            vm.listaProyectosConsultados.forEach(proyectoentidad => {
                                if (proyectoentidad.TipoProyecto == vm.tipoproyecto) {
                                    $sessionStorage.tipoaccion = vm.tipoaccion;
                                    const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                        ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal, TramiteId } = proyectoentidad;
                                    vm.listaEntidadesProy.push({
                                        BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                        ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal, TramiteId
                                    });
                                }
                            });
                        }

                        //vm.listaEntidadesProy = response.data;
                        vm.cantProyectosAsociados = vm.listaEntidadesProy.length;
                        if (vm.tipotramiteid == "99" && vm.cantProyectosAsociados > 0) {//Adicion
                            if ($sessionStorage.tipoaccion == vm.tipoaccion) {
                                $sessionStorage.EstadoAsociacionVF = 'Con Asociación' + vm.tipoproyecto;
                                $sessionStorage.EstadoAjusteCreado = true;
                            }
                            vm.nombreEstadoAsociacion = 'Con proyectos asociados';
                        }

                        if (vm.tipotramiteid == "99" && vm.cantProyectosAsociados == 0) {//Adicion
                            vm.nombreEstadoAsociacion = 'Sin proyectos asociados';
                            if ($sessionStorage.tipoaccion == vm.tipoaccion) {
                                $sessionStorage.EstadoAsociacionVF = 'Sin Asociación' + vm.tipoproyecto;
                                $sessionStorage.EstadoAjusteCreado = false;
                                if (vm.tipoaccion == 'O') {
                                    vm.origenrecursos = 0;
                                    vm.rubrofuncionamiento = '';
                                }

                            }
                        }


                        var listaproy = [];
                        vm.listaEntidadesProy.forEach(entidad => {
                            if (entidad.TipoProyecto === vm.tipoproyecto) {
                                if (!vm.listaGrupoEntidades.find(ent => ent.EntidadId === entidad.EntidadId)) {
                                    const { Sector, NombreEntidad, EntidadId } = entidad;
                                    vm.listaGrupoEntidades.push({ Sector, NombreEntidad, EntidadId });
                                }
                                listaproy.push(entidad.BPIN);
                            }
                        });

                        vm.listaGrupoEntidades.forEach(entidad => {

                            vm.listaGrupoProyectos = [];

                            vm.listaEntidadesProy.forEach(proyectoentidad => {
                                //var MontoProyectoNacion = parseInt(proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion);
                                //var MontoProyectoPropios = parseInt(proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios);
                                //var MontoTramiteNacion = parseInt(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion);
                                //var MontoTramitePropios = parseInt(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios);

                                //if (proyectoentidad.TipoProyecto === 'Credito') {
                                //    vm.ValorTotalMontoRC += (MontoTramiteNacion + MontoTramitePropios);
                                //    vm.ValorTotalMontoNacionRC += (MontoTramiteNacion);
                                //    vm.ValorTotalMontoPropiosRC += (MontoTramitePropios);
                                //}
                                //else {
                                //    vm.ValorTotalMontoRCC += (MontoTramiteNacion + MontoTramitePropios);
                                //    vm.ValorTotalMontoNacionRCC += (MontoTramiteNacion);
                                //    vm.ValorTotalMontoPropiosRCC += (MontoTramitePropios);
                                //}
                                proyectoentidad.ValorMontoProyectoNacion = formatearNumero(proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion);
                                proyectoentidad.ValorMontoProyectoPropios = formatearNumero(proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios);
                                proyectoentidad.ValorMontoTramiteNacion = formatearNumero(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion);
                                proyectoentidad.ValorMontoTramitePropios = formatearNumero(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios);

                                if (proyectoentidad.EntidadId === entidad.EntidadId) {
                                    if (!vm.listaGrupoProyectos.find(p => p.EntidadId === proyectoentidad.EntidadId)) {
                                        const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                            ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal } = proyectoentidad;
                                        vm.listaGrupoProyectos.push({
                                            BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                            ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal
                                        });
                                    }
                                }
                            });
                        });

                        /* Ajuste mientras lucho revisa. Marco Montoya. Bug. 37088. 17/11/2022*/
                        vm.ValorTotalMontoRC = 0;
                        vm.ValorTotalMontoNacionRC = 0;
                        vm.ValorTotalMontoPropiosRC = 0;
                        vm.ValorTotalMontoRCC = 0;
                        vm.ValorTotalMontoNacionRCC = 0;
                        vm.ValorTotalMontoPropiosRCC = 0;
                        //vm.listaEntidadesProy.forEach(proyectoentidad => {

                        //    var MontoTramiteNacion = desFormatearNumero(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion);
                        //    var MontoTramitePropios = desFormatearNumero(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios);

                        //    if (proyectoentidad.TipoProyecto === 'Credito') {
                        //        vm.ValorTotalMontoRC += (MontoTramiteNacion + MontoTramitePropios);
                        //        vm.ValorTotalMontoNacionRC += (MontoTramiteNacion);
                        //        vm.ValorTotalMontoPropiosRC += (MontoTramitePropios);
                        //    }
                        //    else {
                        //        vm.ValorTotalMontoRCC += (MontoTramiteNacion + MontoTramitePropios);
                        //        vm.ValorTotalMontoNacionRCC += (MontoTramiteNacion);
                        //        vm.ValorTotalMontoPropiosRCC += (MontoTramitePropios);
                        //    }
                        //});
                        /* Fin ajuste.*/

                        vm.actualizarMontos(); ///2022-12-17

                        //vm.ValorTotalMontoRC = formatearNumero(vm.ValorTotalMontoRC);
                        //vm.ValorTotalMontoNacionRC = formatearNumero(vm.ValorTotalMontoNacionRC);
                        //vm.ValorTotalMontoPropiosRC = formatearNumero(vm.ValorTotalMontoPropiosRC);
                        //vm.ValorTotalMontoRCC = formatearNumero(vm.ValorTotalMontoRCC);
                        //vm.ValorTotalMontoNacionRCC = formatearNumero(vm.ValorTotalMontoNacionRCC);
                        //vm.ValorTotalMontoPropiosRCC = formatearNumero(vm.ValorTotalMontoPropiosRCC);

                        vm.listaGrupoProyectos = [];
                        vm.listaEntidadesProy.forEach(proyectoentidad => {
                            const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal, TramiteId } = proyectoentidad;
                            vm.listaGrupoProyectos.push({
                                BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal, TramiteId
                            });

                           // vm.gestionarListaProgramasSubs_asociados(proyectoentidad);
                        });

                        vm.listaGrillaProyectos = [];
                        vm.listaEntidadesProy.forEach(entidad => {
                            vm.listaGrillaProyectos.push({
                                entidad: entidad.NombreEntidad,
                                BPIN: entidad.BPIN,
                                NombreProyecto: entidad.NombreProyecto,
                                ProyectoId: entidad.ProyectoId,
                                TipoProyecto: entidad.TipoProyecto,
                                EntidadId: entidad.EntidadId,
                                Estado: entidad.Estado,
                                ValorMontoProyectoNacion: entidad.ValorMontoProyectoNacion === null ? '0' : entidad.ValorMontoProyectoNacion,
                                ValorMontoProyectoPropios: entidad.ValorMontoProyectoPropios === null ? '0' : entidad.ValorMontoProyectoPropios,
                                ValorMontoTramiteNacion: entidad.ValorMontoTramiteNacion === null ? '0' : entidad.ValorMontoTramiteNacion,
                                ValorMontoTramitePropios: entidad.ValorMontoTramitePropios === null ? '0' : entidad.ValorMontoTramitePropios,
                                Programa: entidad.Programa,
                                SubPrograma: entidad.SubPrograma,
                                EstadoActualizacion: entidad.EstadoActualizacion,
                                CodigoPresupuestal: entidad.CodigoPresupuestal,

                            });

                        });

                        comunesServicio.obtenerInstanciasActivasProyectos(listaproy)
                            .then(function (resul) {
                                vm.listaEntidadesProy.forEach(entproy => {
                                    if (resul.data.find(res => res === entproy.BPIN))
                                        entproy.TieneInstancia = true;
                                    else
                                        entproy.TieneInstancia = false;

                                    if (entproy.EstadoActualizacion.toLowerCase().includes('viabilidad definitiva'))
                                        entproy.tieneAmpliadoConcepto = true;
                                    else
                                        entproy.tieneAmpliadoConcepto = false;
                                });
                                //if (resul.data.length === 0)
                                //    entidadx.TieneInstancia = false;
                                ///* retorno = false;*/
                                //else
                                //    entidadx.TieneInstancia = true;
                            }
                            );


                    }
                    else {
                        ////eliminarCapitulosModificados();
                        if (vm.tipotramiteid == "99") {//Adicion
                            //
                            vm.nombreEstadoAsociacion = 'Sin proyectos asociados';
                            $sessionStorage.EstadoAsociacionVF = 'Sin Asociación'
                            //
                            if ($sessionStorage.tipoaccion == vm.tipoaccion) {
                                $sessionStorage.EstadoAsociacionVF = 'Sin Asociación' + vm.tipoproyecto;
                                $sessionStorage.EstadoAjusteCreado = false;
                            }
                        }
                    }
                }).then(function (resultadofinal) {
                    //if (vm.actualizacomponentes != undefined) {
                    //    vm.actualizacomponentes = vm.actualizacomponentes + '1';
                    //}
                    vm.listaValoresAComparar = angular.copy(vm.listaEntidadesProy);
                });
        }

        ////vm.gestionarListaProgramasSubs_asociados = function (proyectoentidad) {
        ////    if (vm.tipotramiteid == 1) {
        ////        if (!$sessionStorage.listadosubprogramas.find(ent => ent.id === proyectoentidad.SubPrograma)) {
        ////            var subprograma = { id: proyectoentidad.SubPrograma };
        ////            $sessionStorage.listadosubprogramas.push(subprograma);
        ////        }
        ////    }
        ////    if (vm.tipotramiteid == 8 && proyectoentidad.TipoProyecto === 'Contracredito')
        ////        $sessionStorage.existecontradistribucion = true;
        ////    if (vm.tipotramiteid == 2) {
        ////        if (!$sessionStorage.listadosubprogramas.find(ent => ent.id === proyectoentidad.SubPrograma)) {
        ////            var subprograma = { id: proyectoentidad.SubPrograma };
        ////            $sessionStorage.listadosubprogramas.push(subprograma);
        ////        }
        ////        if (!$sessionStorage.listadoprogramas.find(ent => ent.id === proyectoentidad.Programa)) {
        ////            var programa = { id: proyectoentidad.Programa };
        ////            $sessionStorage.listadoprogramas.push(programa);
        ////        }
        ////    }
        ////}


        vm.generarInstanciasMasiva = function () {
            var boolproyectossinmontotramite = false;
            var listaproyectos = [];
            vm.continuarMasivo = false;

            vm.variableObligatoriosNoIngresados = localStorage.getItem('contObligatoriosNoIngresados');
            vm.continuarMasivo = true;
            //if (vm.variableObligatoriosNoIngresados === '0') {
            //    vm.continuarMasivo = true;
            //}
            //else
            //    utilidades.mensajeError("No se han ingresado todos los documentos soporte obligatorios para el trámite!");
            if ($sessionStorage.sessionDocumentos < 100 || $sessionStorage.sessionDocumentos === undefined) {
                utilidades.mensajeError(" Todos los documentos soporte deben estar asociados al trámite!");
            }
            else {
                if (vm.continuarMasivo == true) {
                    vm.listaGrillaProyectos.map(function (item) {
                        if (parseInt(limpiaNumero(item.ValorMontoTramiteNacion)) <= 0 && parseInt(limpiaNumero(item.ValorMontoTramitePropios)) <= 0) {
                            boolproyectossinmontotramite = true;
                        }
                        else
                            listaproyectos.push(item.BPIN);
                    });

                    if (boolproyectossinmontotramite)
                        utilidades.mensajeError("Hay proyectos que no tienen monto tramite!");
                    else {

                        var listaparagenerar = [];
                        var listaqueyatiene = [];
                        comunesServicio.obtenerInstanciasActivasProyectos(listaproyectos)
                            .then(function (resultado) {
                                listaqueyatiene = resultado.data;
                                vm.listaGrillaProyectos.map(function (item) {
                                    var tmp = listaqueyatiene.find(x => x === item.BPIN);
                                    if (tmp === undefined)
                                        listaparagenerar.push(item);
                                })

                            }).then(function (result) {
                                if (listaparagenerar.length <= 0)
                                    utilidades.mensajeError("Todos los proyectos ya tienen generada la instancia!");
                                else
                                    generarInstanciaMasivo(listaparagenerar);
                            });
                    }
                }
            }
        }

        function generarInstanciaMasivo(listaparagenerar) {
            var cargo = false;
            var listatramites = [];
            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];
            listaparagenerar.map(function (item) {
                var tramiteDto = {
                    FlujoId: vm.idflujo,
                    ObjetoId: item.BPIN,
                    UsuarioId: vm.peticionObtenerInbox.IdUsuario,
                    RolId: usuarioRolId,
                    TipoObjetoId: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',  //proyecto
                    ListaEntidades: [item.EntidadId],
                    IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                    Proyectos: [{
                        IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                        IdTipoObjeto: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',
                        IdObjetoNegocio: item.BPIN,
                        IdEntidad: item.EntidadId,
                        IdAccion: '',
                        ProyectoId: item.ProyectoId,
                        FlujoId: vm.idflujo
                    }]
                };
                listatramites.push(tramiteDto);

            });

            flujoServicios.generarInstanciaMasivo(listatramites)
                .then(function (resultado) {
                    if (!resultado.length) {
                        utilidades.mensajeError('No se creó la instancia');
                        return;
                    }
                    var instanciasFallidas = resultado.filter(function (instancia) {
                        return !instancia.Exitoso;
                    });
                    var cantidadInstanciasFallidas = instanciasFallidas.length;

                    if (cantidadInstanciasFallidas) {
                        utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                    } else {
                        listaparagenerar.map(function (item) {
                            // var tmpresult = resultado.find(x => x.id);
                            ActualizarInstanciaProyecto(item, resultado);
                            cargo = true;
                        })

                    }

                },
                    function (error) {
                        if (error) {
                            utilidades.mensajeError(error);
                        }
                    }).then(function (result) {
                        if (cargo) {
                            vm.actualizadetalle++;
                            utilidades.mensajeSuccess('Se crearon instancias exitosamente');
                        }
                    });;

        }

        function eliminarAsociacion(proyectoId, bpin, tipoProyecto) {
            if (proyectoId === undefined || proyectoId === '' || proyectoId === 0) {
                utilidades.mensajeError('No hay proyectos asociados.');
            }
            else {
                utilidades.mensajeWarning("Esto eliminará la información correspondiente al BPIN " + bpin
                    + " incluida en este proceso de trámite, mas la consignada en el proceso de Ajustes del proyecto. ¿Esta seguro de continuar?",
                    function funcionContinuar() {
                        var eliminarAsociacionDto = {
                            TramiteId: vm.tramiteid,//$sessionStorage.tramiteId,
                            InstanciaId: $sessionStorage.idInstancia,
                            ProyectoId: proyectoId,
                            IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                        };

                        //tramiteVigenciaFuturaServicio.eliminarAsociacion(eliminarAsociacionDto)
                        trasladosServicio.eliminarProyectoTramite(eliminarAsociacionDto, eliminarAsociacionDto)
                            .then(function (response) {
                                if (response.status == "200") {
                                    $sessionStorage.tipoaccion = vm.tipoaccion;
                                    vm.actualizadetalle++;
                                    vm.actualizacomponentes++;
                                    utilidades.mensajeSuccess("Se elimino la información del proyecto con BPIN " + bpin
                                        + " desde este proceso de trámite", false, false, false, "El proyecto ha sido desasociado con éxito.");
                                    ////if (vm.tipotramiteid == 8 && tipoProyecto === 'Contracredito')
                                    ////    $sessionStorage.existecontradistribucion = false;
                                    ////$sessionStorage.listadosubprogramas = [];
                                    ////$sessionStorage.listadoprogramas = [];
                                    var proyAdelete = vm.listaEntidadesProy.find(x => x.ProyectoId === proyectoId);
                                    vm.listaEntidadesProy.splice(proyAdelete, 1);

                                    if (vm.tipotramiteid === '9') {
                                        trasladosServicio.eliminarInstanciaCerrada_AbiertaProyectoTramite($sessionStorage.idInstanciaIframe, proyectoId)
                                            .then(function (result) {
                                                if (result.status === 200) {

                                                    if (vm.listaEntidadesProy == 0)
                                                        eliminarCapitulosModificados();

                                                    var texto = "El proyecto con BPIN " + bpin + " ha sido desasociado del tramite " + $sessionStorage.idObjetoNegocio;
                                                    comunesServicio.notificarUsuariosPorInstanciaPadre($sessionStorage.idInstanciaIframe, "Desasociar proyecto", texto);
                                                }
                                            });
                                    }
                                    ////vm.listaEntidadesProy.forEach(proyectoentidad => {
                                    ////    vm.gestionarListaProgramasSubs_asociados(proyectoentidad);
                                    ////});

                                    //Es posible que para el proceso de creación de Ajuste y validaciones se requiera el siguiente código. la guia de como implementarlo la encuentran en el archivo
                                    // DNP.Backbone.Web.UI\src\app\formulario\ventanas\tramiteVigenciaFutura\componentes\asociarProyectovf\seleccionProyecto\seleccionProyectosvfController.js

                                    ////para guardar los capitulos modificados y que se llenen las lunas, este modificado en 0
                                    //ObtenerSeccionCapitulo();
                                    //eliminarCapitulosModificados(vm.seccionCapituloAsociarid);
                                    //eliminarCapitulosModificados(vm.seccionCapituloCrear);
                                    //$sessionStorage.EstadoAjusteCreado = false;
                                    //vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                                    //$sessionStorage.EstadoAjusteFinalizado = false;
                                    //vm.estadoAjusteFinalizado = $sessionStorage.EstadoAjusteFinalizado;
                                    /////Ajuste Estabilizacion
                                    //// $sessionStorage.proyectoId = 0;
                                    //$sessionStorage.BPIN = '';
                                    //$sessionStorage.nombreProyecto = '';
                                    //vm.BPIN = $sessionStorage.BPIN;
                                    //vm.proyecto = $sessionStorage.nombreProyecto;
                                }
                                else {
                                    //utilidades.mensajeError("Error al realizar la operación", false);
                                }
                            })
                            .catch(error => {
                                if (error.status == "400") {
                                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                    return;
                                }
                                utilidades.mensajeError("Error al realizar la operación");
                            });
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "El proyecto será desasociado."
                )
            }
        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        vm.iniciarInstanciaProyecto = function (entity) {
            if (!vm.visibleValidar) {
                return;
            }

            vm.variableObligatoriosNoIngresados = localStorage.getItem('contObligatoriosNoIngresados');
            try {

                var montotramitenacion = parseInt(entity.ValorMontoTramiteNacion);
                var montotramitepropio = parseInt(entity.ValorMontoTramitePropios);
                var montoTramite = montotramitenacion + montotramitepropio;
                var montoproyecto = parseInt(limpiaNumero(entity.ValorMontoProyectoPropios)) + parseInt(limpiaNumero(entity.ValorMontoProyectoNacion));

                if (montoTramite === 0 || montoTramite === "0" || montoTramite === "") {
                    utilidades.mensajeError("No ha ingresado el monto del trámite!");
                } else {
                    if (parseInt(montoTramite, 10) > parseInt(montoproyecto, 0) && entity.TipoProyecto === "Contracredito") {
                        utilidades.mensajeError("El monto del trámite no puede ser mayor al monto del proyecto!");
                    } else {
                        //ActualizarValoresProyecto(entity, 0);
                        var listaproyectos = [];
                        listaproyectos.push(entity.BPIN);
                        comunesServicio.obtenerInstanciasActivasProyectos(listaproyectos)
                            .then(function (resultado) {
                                if (resultado.data.length === 0)
                                    //if (vm.variableObligatoriosNoIngresados === '0') {
                                    generarInstancia(entity);
                                //}
                                //else
                                //   utilidades.mensajeError("No se han ingresado todos los documentos soporte obligatorios para el trámite!");
                                else
                                    utilidades.mensajeError("El proyecto ya tiene instacia generada!");
                            });
                    }
                }
            }
            catch (exception) {
                console.log('controladorProyecto.btnIniciarInstanciaProyecto_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };

        function generarInstancia(proyecto) {
            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];

            if ($sessionStorage.sessionDocumentos < 100 || $sessionStorage.sessionDocumentos === undefined) {
                utilidades.mensajeError(" Todos los documentos soporte deben estar asociados al trámite!");
            }
            else {
                const tramiteDto = {
                    FlujoId: vm.idflujo,
                    ObjetoId: proyecto.BPIN,
                    UsuarioId: vm.peticionObtenerInbox.IdUsuario,
                    RolId: usuarioRolId,
                    TipoObjetoId: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',  //proyecto
                    ListaEntidades: [proyecto.EntidadId],
                    IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                    Proyectos: [{
                        IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                        IdTipoObjeto: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',
                        IdObjetoNegocio: proyecto.BPIN,
                        IdEntidad: proyecto.EntidadId,
                        IdAccion: '',
                        ProyectoId: proyecto.ProyectoId,
                        FlujoId: vm.idflujo
                    }]
                };

                flujoServicios.generarInstancia(tramiteDto)
                    .then(function (resultado) {
                        if (!resultado.length) {
                            utilidades.mensajeError('No se creó la instancia');
                            return;
                        }
                        var instanciasFallidas = resultado.filter(function (instancia) {
                            return !instancia.Exitoso;
                        });
                        var cantidadInstanciasFallidas = instanciasFallidas.length;

                        if (cantidadInstanciasFallidas) {
                            utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                        } else {
                            ActualizarInstanciaProyecto(proyecto, resultado);
                            // utilidades.mensajeSuccess('Se crearon instancias exitosamente');
                            vm.actualizadetalle++;
                            utilidades.mensajeSuccess('Recuerde que para la activación del proceso es necesario diligenciar y guardar por la línea de información o proyecto al menos uno de los campos (Nación o Propios).', false, false, false, "El curso del proceso se ha iniciado con éxito. Sus datos se reflejarán en otros espacios de este formulario.");
                        }

                    });
            }

        }

        function ActualizarInstanciaProyecto(proyecto, resultado) {

            var montotramitenacion = parseInt(proyecto.ValorMontoTramiteNacion.replaceAll(".", ""));
            var montotramitepropio = parseInt(proyecto.ValorMontoTramitePropios.replaceAll(".", ""));
            var montoTramite = montotramitenacion + montotramitepropio;

            vm.listaIstanciasCreadas = [];
            vm.listaIstanciasCreadas = resultado;

            vm.listaIstanciasCreadas.forEach(instancia => {
                const proyectoTramiteDto = {
                    TramiteId: vm.TramiteId,
                    InstanciaId: instancia.InstanciaId,
                    ProyectoId: proyecto.ProyectoId,
                    EntidadId: proyecto.EntidadId,
                    TipoRolId: 1,
                    ValorMontoEnTramite: limpiaNumero(montoTramite)
                };

                comunesServicio.ActualizarInstanciaProyecto(proyectoTramiteDto).then(
                    function (resultado) {

                        if (resultado.data && (resultado.statusText === "OK" || resultado.status === 200)) {
                            //utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            utilidades.mensajeSuccess('Recuerde que para la activación de los procesos es necesario diligenciar y guardar por línea de información o proyecto al menos uno de los campos (Nación o Propios).', false, false, false, "El curso de procesos se ha iniciado con éxito. Los datos se reflejarán en otros espacios de este formulario.");
                            //generarInstancia(proyecto);
                        } else {
                            swal('', "Error al actualizar la instancia en el proyecto", 'error');
                        }
                    },
                    function (error) {
                        if (error) {
                            utilidades.mensajeError(error);
                        }
                    }
                );
            });
        }

        function actualizaEstado(bpin) {
            vm.ajuste = 0;
            vm.BPIN = bpin;
            if (vm.model !== undefined && vm.model.observacion !== undefined) {
                vm.model.observacion = '';
            }

            utilidades.mensajeWarning(
                "La ampliación del concepto se reflejá en la pestaña Justificación de este formulario, cuando se responda a esta solicitud desde el formulario Ajuste del proyecto.<br/>¿Está seguro de continuar? ",
                function funcionContinuar() {
                    vm.tituloMensaje = "Solicitud para ampliar concepto";
                    vm.mensaje = "Escriba sus razones de devolver el formularío para ampliar concepto* ";
                    angular.element('#IPModal').modal('show');
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El proyecto se devolverá a Ajuste del proyecto para ampliar concepto");
        }


        function aceptar() {

            if (vm.model !== undefined && vm.model.observacion !== undefined) {

                //Cierra la pantalla modal
                angular.element('#IPModal').modal('hide');

                if (vm.BPINesParaAjustarValores.length != 0) {
                    vm.BPINesParaAjustarValores.forEach(x => {
                        modalActualizaEstadoAjusteProyectoServicio.actualizaEstadoAjusteProyecto(1, x, $sessionStorage.tramiteId, vm.model.observacion)
                            .then(function (response) {
                                let exito = response.data.Exito;

                                if (exito) {
                                    let mensajeTitulo = "", mensaje = "";

                                    vm.conceptoHabilitado = true;
                                    vm.valorHabilitado = true;
                                    mensajeTitulo = "El formulario se ha devuelto al paso 1 Crear trámite. <br/>  El proyecto se ha devuelto a Control  posterior";
                                    mensaje = "Usted puede acceder a este proceso desde la consola  de procesos."

                                    utilidades.mensajeSuccess(mensaje, false, false, false, mensajeTitulo);
                                    $location.path("/tramites/ej");
                                    vm.BPIN = '';
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
                    });

                    vm.BPINesParaAjustarValores = [];
                }
                else {

                    modalActualizaEstadoAjusteProyectoServicio.actualizaEstadoAjusteProyecto(vm.ajuste, vm.BPIN, $sessionStorage.tramiteId, vm.model.observacion)
                        .then(function (response) {
                            let exito = response.data.Exito;

                            if (exito) {
                                let mensajeTitulo = "", mensaje = "";

                                vm.conceptoHabilitado = true;
                                mensajeTitulo = "El proyecto fue devuelto al formulario de Ajustes del proyecto";

                                utilidades.mensajeSuccess(mensaje, false, false, false, mensajeTitulo);

                                ObtenerProyectosTramite();
                                $sessionStorage.EstadoDNpAplicado = false;
                                vm.callback();

                                vm.BPIN = '';
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
            }
            else {
                utilidades.mensajeError("Incluir la observación", false); return false;
            }
        }


        function cancelar() {
            $('#tablatest input[type="checkbox"]:checked').each(function () {
                $(this).prop('checked', false);
            });
        }

        function seleccionarTodos() {
            $('#tablatest input[type="checkbox"]').each(function () {
                $(this).prop('checked', true);
            });
        }

        function devolverSeleccion() {
            var selected = new Array();
            $('#tablatest input[type="checkbox"]:checked').each(function () {
                selected.push($(this).attr('id'));
            });

            var entities = vm.listaEntidadesProy.filter(entity => selected.includes(entity.ProyectoId.toString()));


            if (vm.model !== undefined && vm.model.observacion !== undefined) {
                vm.model.observacion = '';
            }

            vm.BPINesParaAjustarValores = entities.map(entidad => entidad.BPIN);

            utilidades.mensajeWarning(
                "Este formulario desaparecerá  de sus procesos pendientes hasta  que reciba  nuevamente  los permisos de edición.<br/>¿Está seguro de continuar? ",
                function funcionContinuar() {
                    vm.tituloMensaje = "Solicitud para ajustar valores";
                    vm.mensaje = "Escriba sus razones de devolver el formularío para ajustar valores*";

                    angular.element('#IPModal').modal('show');
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El proyecto se devolverá al paso 1 Crear Trámite.<br/> El proyecto se devolvera a control posterior");

            console.log(selected);
        }



        function formatearNumero(value) {
            var numerotmp = (value == '' || Number.isNaN(value)) ? 0 : value.toString();
            //No se contemplaran decimales, por ahora 21/12/2022
            //var numbe = parseFloat(numerotmp).toString().replace(".", ",");
            var numbe = parseInt(numerotmp).toString().replace(".", "");
            return numbe.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }

        function desFormatearNumero(value) {
            //No se contemplaran decimales, por ahora 21/12/2022
            //if (value.toString().split(',').length > 1) {                
            //    return Number(value.toString().split(',')[0].replace(/[^0-9,-]+/g, "")).toString() + ',' + value.toString().split(',')[1];
            //}
            //else
            return (value == '' || value == 'NaN') ? 0 : Number(value.replace(/[^0-9,-]+/g, ""));
        }

        function getIdEtapa() {
            var idEtapa = [];
            switch (vm.etapa) {
                case 'pl':
                    idEtapa = [constantesBackbone.idEtapaPlaneacion, constantesBackbone.idEtapaViabilidadRegistro, constantesBackbone.idEtapaAjustes, constantesBackbone.idEtapaPriorizacion];
                    break;
                case 'pr':
                    idEtapa = [constantesBackbone.idEtapaProgramacion];
                    break;
                case 'gr':
                    idEtapa = [constantesBackbone.idEtapaGestionRecursos, constantesBackbone.idEtapaSolicitudRecursos, constantesBackbone.idEtapaRevisionRequisitos, constantesBackbone.idEtapaAprobacion, constantesBackbone.idEtapaAjustesGR];
                    break;
                case 'ej':
                    idEtapa = [constantesBackbone.idEtapaEjecucion, constantesBackbone.idEtapaNuevaEjecucion, constantesBackbone.idEtapaProgramacionEjecucion, constantesBackbone.idEtapaAjustesEjecucion, constantesBackbone.idEtapaTramitesEjecucion, constantesBackbone.idEtapaSeguimientoControl];
                    break;
                case 'se':
                    idEtapa = [];
                    break;
                case 'ev':
                    idEtapa = [constantesBackbone.idEtapaEvaluacion, constantesBackbone.idEtapaCortoPlazo, constantesBackbone.idEtapaMedianoPlazo, constantesBackbone.idEtapaLargoPlazo];
                    break;
            }
            return idEtapa;
        }

        vm.getEstiloBtnGuardar = function () {
            if (vm.disabled == false)
                return "btnguardarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        vm.getEstiloBtnEditar = function () {
            if (vm.cantProyectosAsociados > 0)
                return "btneditarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        vm.ActivarEditar = function () {
            var panel = document.getElementById('Guardar');
            if (vm.disabled == true) {
                vm.Editar = "CANCELAR";
                vm.disabled = false;
                angular.forEach(vm.listaEntidadesProy, function (series) {
                    if (series.TipoProyecto == vm.tipoproyecto) {
                        series.ValorMontoTramiteNacionAnterior = series.ValorMontoTramiteNacion;
                        series.ValorMontoTramitePropiosAnterior = series.ValorMontoTramitePropios;
                    }

                });
                //panel.classList.replace("btnguardarDisabledDNP", "btnguardarDNP");
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabled = true;
                angular.forEach(vm.listaEntidadesProy, function (series) {
                    if (series.TipoProyecto == vm.tipoproyecto) {
                        series.ValorMontoTramiteNacion = series.ValorMontoTramiteNacionAnterior;
                        series.ValorMontoTramitePropios = series.ValorMontoTramitePropiosAnterior;
                    }
                });
                vm.actualizarMontos();
                //panel.classList.replace("btnguardarDNP", "btnguardarDisabledDNP");
                ////ObtenerProyectosTramite();
            }
        }

        vm.focus = function (event) {
            event.target.value = event.target.value.length > 0 ? desFormatearNumero(event.target.value) : '';
        }

        vm.actualizarMontos = function (event, fila, valorIndex) {
            var sumPropiosRC = 0;
            var sumPropiosRCC = 0;
            var sumNacionRC = 0;
            var sumNacionRCC = 0;
            var sumTotalRC = 0;
            var sumTotalRCC = 0;
            var valMontoPropios = 0;

            vm.listaEntidadesProy.forEach(entProyecto => {

                var MontoTramiteNacion = parseInt((entProyecto.ValorMontoTramiteNacion === null || entProyecto.ValorMontoTramiteNacion === '') ? '0' : entProyecto.ValorMontoTramiteNacion.toString().replace(/[^0-9,-]+/g, ""));
                var MontoTramitePropios = parseInt((entProyecto.ValorMontoTramitePropios === null || entProyecto.ValorMontoTramitePropios === '') ? '0' : entProyecto.ValorMontoTramitePropios.toString().replace(/[^0-9,-]+/g, ""));
                //No se contemplaran decimales, por ahora 21/12/2022
                //if (entProyecto.ValorMontoTramiteNacion.toString().split(',').length > 1)
                //    MontoTramiteNacion = parseFloat(entProyecto.ValorMontoTramiteNacion.toString().replace(",", "."));
                //if (entProyecto.ValorMontoTramitePropios.toString().split(',').length>1)
                //    MontoTramitePropios = parseFloat(entProyecto.ValorMontoTramitePropios.toString().replace(",", "."));

                if (!vm.unSoloTipoOperacion) {
                    if (entProyecto.TipoProyecto === 'Credito') {
                        sumTotalRC += (MontoTramiteNacion + MontoTramitePropios);
                        sumNacionRC += (MontoTramiteNacion);
                        sumPropiosRC += (MontoTramitePropios);
                    }
                    else {
                        sumTotalRCC += (MontoTramiteNacion + MontoTramitePropios);
                        sumNacionRCC += (MontoTramiteNacion);
                        sumPropiosRCC += (MontoTramitePropios);
                    }
                }
                else {
                    sumTotalRCC += (MontoTramiteNacion + MontoTramitePropios);
                    sumNacionRCC += (MontoTramiteNacion);
                    sumPropiosRCC += (MontoTramitePropios);
                }
                if (event == undefined) {
                    entProyecto.ValorMontoTramiteNacion = formatearNumero(MontoTramiteNacion);
                    entProyecto.ValorMontoTramitePropios = formatearNumero(MontoTramitePropios);
                }
                //No se contemplaran decimales, por ahora 21/12/2022
                //if (event == undefined) {
                //    //NACION
                //    if (entProyecto.ValorMontoTramiteNacion.split(',').length > 1) {
                //        entProyecto.ValorMontoTramiteNacion = formatearNumero(parseFloat(entProyecto.ValorMontoTramiteNacion.toString().replace(",", ".")));
                //    }
                //    else if (!isNaN(parseFloat(entProyecto.ValorMontoTramiteNacion)))
                //        entProyecto.ValorMontoTramiteNacion = entProyecto.ValorMontoTramiteNacion;
                //    else
                //        entProyecto.ValorMontoTramiteNacion = '';
                //    //PROPIOS
                //    if (entProyecto.ValorMontoTramitePropios.split(',').length > 1) {
                //        entProyecto.ValorMontoTramitePropios = formatearNumero(parseFloat(entProyecto.ValorMontoTramitePropios.toString().replace(",", ".")));
                //    }
                //    else if (!isNaN(parseFloat(entProyecto.ValorMontoTramitePropios)))
                //        entProyecto.ValorMontoTramitePropios = entProyecto.ValorMontoTramitePropios;
                //    else
                //        entProyecto.ValorMontoTramitePropios = '';
                //}

            });
            vm.totalTramite = sumTotalRCC + sumTotalRC;
            vm.ValorTotalMontoRC = formatearNumero(sumTotalRC);
            vm.ValorTotalMontoNacionRC = formatearNumero(sumNacionRC);
            vm.ValorTotalMontoPropiosRC = formatearNumero(sumPropiosRC);
            vm.ValorTotalMontoRCC = formatearNumero(sumTotalRCC);
            vm.ValorTotalMontoNacionRCC = formatearNumero(sumNacionRCC);
            vm.ValorTotalMontoPropiosRCC = formatearNumero(sumPropiosRCC);

            if (event != undefined) {
                //No se contemplaran decimales, por ahora 21/12/2022
                //if (event.target.defaultValue.split(',').length > 1) {
                //    event.target.value = formatearNumero(parseFloat(event.target.defaultValue.toString().replace(",", ".")));
                //}
                //else
                if (!isNaN(parseInt(event.target.value)))
                    event.target.value = formatearNumero(event.target.value);
                else
                    event.target.value = '';
            }
        };

        vm.validateFormat = function (event) {
            vm.validateFormat = function (event) {

                if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                    event.preventDefault();
                }

                var newValue = event.target.value;
                var spiltArray = String(newValue).split("");
                var tamanioPermitido = 11;
                var tamanio = event.target.value.length;
                var permitido = false;
                permitido = event.target.value.toString().includes(".");
                if (permitido) {
                    tamanioPermitido = 30;

                    var n = String(newValue).split(".");
                    if (n[1]) {
                        var n2 = n[1].slice(0, 4);
                        newValue = [n[0], n2].join(".");
                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        if (n[1].length > 4) {
                            tamanioPermitido = n[0].length + 4;
                            event.target.value = n[0] + '.' + n[1].slice(0, 4);
                            return;
                        }

                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }
                } else {
                    if (tamanio > 12 && event.keyCode != 44) {
                        event.preventDefault();
                    }
                }

                if (event.keyCode === 44 && tamanio == 12) {
                }
                else {
                    if (tamanio > tamanioPermitido || tamanio > 16) {
                        event.preventDefault();
                    }
                }
            }
        }

        vm.actualizarValores = function (response) {
            var tramiteAjustePaso = 0;
            var listaproyectosarevisar = [];
            var listaproyectosSinCondicion = [];
            var continuarActualiza = true;

            if (vm.origenrecursos == 0 && vm.tipoaccion == 'O') {
                utilidades.mensajeWarning(" Debe seleccionar el origen de los recursos para los proyectos aportantes.");
                continuarActualiza = false;
            }

            if (vm.unSoloTipoOperacion && vm.tipotramiteid === "9") {
                var totalVigenteConvenio = 0;

                $sessionStorage.listaDatosAdicion.forEach(incorp => {
                    var Montovigente = parseFloat(incorp.ValorConvenioVigencia);
                    totalVigenteConvenio += Montovigente;
                });
                if (vm.totalTramite > totalVigenteConvenio) {
                    utilidades.mensajeWarning("La suma de valores del tramite no puede ser superior a los valores vigentes del convenio.");
                    continuarActualiza = false;
                }
            }

            if (continuarActualiza == true) {
                vm.listaValoresAComparar.map(function (itemProyecto) {
                    var proyectotmp = vm.listaEntidadesProy.find(x => x.ProyectoId === itemProyecto.ProyectoId);
                    if (proyectotmp.ValorMontoTramiteNacion !== formatearNumero(itemProyecto.ValorMontoTramiteNacion) ||
                        proyectotmp.ValorMontoTramitePropios !== formatearNumero(itemProyecto.ValorMontoTramitePropios)) {

                        listaproyectosarevisar.push(proyectotmp.ProyectoId); //98149
                        listaproyectosSinCondicion.push(itemProyecto);
                    }
                });

                //if (listaproyectosSinCondicion.length === 1) {
                //    comunesServicio.obtenerPasoAjuste(vm.tramiteid, listaproyectosSinCondicion[0].ProyectoId).then(
                //        function (rta) {
                //            tramiteAjustePaso = rta.data;
                //        }).then(function () {
                //            if (tramiteAjustePaso == 2) {
                //                utilidades.mensajeError(" El proyecto " + listaproyectosSinCondicion[0].ProyectoId + " con instancia de ajuste diferente al paso uno!");
                //                //vm.listaEntidadesProy = angular.copy(vm.listaValoresAComparar);
                //                ObtenerProyectosTramite();
                //            }
                //            else
                //                completarActualizarValores();
                //        });
                //}
                //else if (listaproyectosSinCondicion.length > 1) {
                //    var actualice = true;
                //    listaproyectosarevisar.map(function (itemp) {
                //        comunesServicio.obtenerPasoAjuste(vm.tramiteid, itemp).then(
                //            function (rta) {
                //                tramiteAjustePaso = rta.data;
                //                if (tramiteAjustePaso == 2) {
                //                    if (vm.tipoaccion !== 'O') {
                //                        actualice = false;
                //                        utilidades.mensajeError(" Existe al menos un proyecto con instancia de ajuste diferente al paso uno!");
                //                        vm.listaEntidadesProy = angular.copy(vm.listaValoresAComparar);
                //                        return;
                //                    }

                //                }
                //            }).then(function () {

                //                if (actualice)
                //                    completarActualizarValores();
                //            });

                //    });

                //}
                //else {
                completarActualizarValores();
                //}

                vm.Editar = "EDITAR";
                vm.disabled = true;
            }
        };

        vm.formNumero = function (value) {
            return Number(value.toString().replace(/[^0-9,-]+/g, ""));
        }


        function completarActualizarValores() {
            var validarValores = 0;
            var montoCompararNacion;
            var montoCompararPropios;
            var conteoContracredito = 0;

            comunesServicio.obtenerDatosProyectosPorTramite(vm.tramiteid).then(
                function (respuesta) {
                    vm.datosproyecto = respuesta.data;
                }).then(function () {

                    vm.listaEntidadesProy.forEach(proyectoentidad => {
                        proyectoentidad.ValorMontoProyectoNacion = parseInt(proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion.toString().replace(/[^0-9,-]+/g, ""));
                        proyectoentidad.ValorMontoProyectoPropios = parseInt(proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios.toString().replace(/[^0-9,-]+/g, ""));
                        if (proyectoentidad.ValorMontoTramiteNacion.toString().split(',').length > 1)
                            proyectoentidad.ValorMontoTramiteNacion = parseFloat(proyectoentidad.ValorMontoTramiteNacion.toString().replace(",", "."));
                        else
                            proyectoentidad.ValorMontoTramiteNacion = parseInt(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion.toString().replace(/[^0-9,-]+/g, ""));
                        if (proyectoentidad.ValorMontoTramitePropios.toString().split(',').length > 1)
                            proyectoentidad.ValorMontoTramitePropios = parseFloat(proyectoentidad.ValorMontoTramitePropios.toString().replace(",", "."));
                        else
                            proyectoentidad.ValorMontoTramitePropios = parseInt(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios.toString().replace(/[^0-9,-]+/g, ""));

                        if (proyectoentidad.TipoProyecto === 'Contracredito') {
                            conteoContracredito++;
                            if (vm.datosproyecto.find(dp => dp.ProyectoId === proyectoentidad.ProyectoId)) {

                                var vigenciasFuturas = vm.datosproyecto.find(vf => vf.ProyectoId === proyectoentidad.ProyectoId)

                                montoCompararNacion = proyectoentidad.ValorMontoProyectoNacion - vigenciasFuturas.ValorVigenciaFuturaNacion;
                                montoCompararPropios = proyectoentidad.ValorMontoProyectoPropios - vigenciasFuturas.ValorVigenciaFuturaPropios;

                            }
                            else {
                                montoCompararNacion = proyectoentidad.ValorMontoProyectoNacion;
                                montoCompararPropios = proyectoentidad.ValorMontoProyectoPropios;
                            }

                            if (parseInt(proyectoentidad.ValorMontoTramiteNacion, 10) > parseInt(montoCompararNacion, 0) || parseInt(proyectoentidad.ValorMontoTramitePropios, 10) > parseInt(montoCompararPropios, 0)) {
                                validarValores = 0;
                            }
                            else validarValores++;
                        }
                    })
                }).then(function () {

                    if (conteoContracredito === validarValores) {
                        comunesServicio.guardarMontosTramite(vm.listaEntidadesProy).then(function (response) {
                            if (response.data && (response.statusText === "OK" || response.status === 200)) {
                                utilidades.mensajeSuccess('Usted puede iniciar el curso del proceso mediante el ícono activo de la fila "iniciar proceso" ', false, false, false, "Los datos fueron guardados con éxito.");
                                //ObtenerProyectosTramite();
                                vm.actualizadetalle++;
                                vm.actualizarMontos();

                                if (vm.unSoloTipoOperacion && vm.tipotramiteid === "9") {

                                    if (vm.origenrecursos != 2) {
                                        vm.rubrofuncionamiento = '';
                                    }

                                    let origenRecurso = {
                                        TramiteId: vm.tramiteid,
                                        TipoOrigenId: vm.origenrecursos,
                                        Rubro: vm.rubrofuncionamiento
                                    };

                                    comunesServicio.setOrigenRecursosTramite(origenRecurso).then(function (response) {
                                        getOrigenRecursosTramite();
                                    });
                                }

                            } else {
                                swal('', "Error al realizar la operación", 'error');
                            }
                        });
                    }
                    else {
                        utilidades.mensajeError("Los montos de los trámites no pueden ser mayores que los valores disponibles para proyectos con tipo de movimiento contracédito!");
                    }
                });
        }

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;


        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");
        }

        function getOrigenRecursosTramite() {
            comunesServicio.getOrigenRecursosTramite(vm.tramiteid)
                .then(function (response) {
                    if (response.data !== null) {
                        console.log(response);
                        vm.origenrecursos = response.data.TipoOrigenId;
                        vm.rubrofuncionamiento = response.data.Rubro;
                    }
                });
        }

        function changeVerifica(valor) {
            vm.origenrecursos = valor;
        }

    }

    angular.module('backbone').component('detalleProyectosUnTipoFormularioSgp', {
        templateUrl: "src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarProyectosUnTipo/detalleProyectosUnTipo/detalleProyectosUnTipoFormularioSgp.html",
        controller: detalleProyectosUnTipoFormularioSgp,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            deshabilitarBotonDevolverSeccionProyecto: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            actualizadetalle: '@',
            idflujo: '@',
            actualizacomponentes: '=',
            nombrecomponentepaso: '@',
            notificacionvalidacion: '&',
            deshabilitar: '@',
            rolanalista: '@',
            tipoaccion: '@',
            tipoproyecto: '@',
            unsolotipooperacion: '@'
        }
    });

})();
