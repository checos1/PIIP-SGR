
(function () {
    'use strict';

    trasladosAccionesController.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        'utilidades',
        'constantesCondicionFiltro',
        '$sessionStorage',
        'flujoServicios',
        'constantesBackbone',
        'trasladosServicio',
        '$routeParams',
        '$route',
        'servicioCargaDatos',
        '$timeout',
        '$location',
        '$q'
    ];



    function trasladosAccionesController(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        utilidades,
        constantesCondicionFiltro,
        $sessionStorage,
        flujoServicios,
        constantesBackbone,
        trasladosServicio,
        $routeParams,
        $route,
        servicioCargaDatos,
        $timeout,
        $location,
        $q
    ) {
        var vm = this;
        vm.user = {};
        //vm.idInstancia = $sessionStorage.idInstanciaIframe;
        vm.noJefePlaneacion = !$sessionStorage.jefePlaneacion;
        vm.disabledJefePlaneacion = $sessionStorage.jefePlaneacion;
        $sessionStorage.director = false;
        /**preuba */
        vm.TramiteId = 0;

        vm.Bpin =  $sessionStorage.BPIN;
        vm.EntidadId = $sessionStorage.EntidadId;
        vm.TipoTramiteId = $sessionStorage.TipoTramiteId;
        vm.NombreProyecto = $sessionStorage.NombreProyecto;


        vm.Bpin = $sessionStorage.BPIN;
        vm.TipoProyecto = $sessionStorage.TipoProyecto;
        vm.EntidadId = $sessionStorage.EntidadId;
        vm.TipoTramiteId = $sessionStorage.TipoTramiteId;
        vm.NombreProyecto = $sessionStorage.NombreProyecto;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.nombreEntidad = $sessionStorage.nombreEntidad;
        vm.nombreTipoTramite = $sessionStorage.nombreTipoTramite;
        //vm.TipoProyecto = 'Contracredito';  
        //vm.ProyectoId = 67056;//  $sessionStorage.ProyectoId;
        //vm.TramiteId = 26;// $sessionStorage.TramiteId;

        vm.ProyectoId = $sessionStorage.ProyectoId;
        vm.TramiteId = $sessionStorage.TramiteId;

        vm.idProyectoTramite = ''; //$sessionStorage.TramiteId;
        vm.idFuentePresupuestal = 0;
        vm.idProyectoRequisito = 0;
        vm.ValorMontoNacion = $sessionStorage.MontoTramiteNacion ? $sessionStorage.MontoTramiteNacion : 0;
        vm.ValorMontoPropios = $sessionStorage.MontoTramitePropios ? $sessionStorage.MontoTramitePropios : 0;
        vm.valorFuenteNacion = formatearNumero(vm.ValorMontoNacion);
        vm.valorFuentePropios = formatearNumero(vm.ValorMontoPropios);
        vm.valorFuenteVigencia = formatearNumero($sessionStorage.ValorMontoProyecto ? $sessionStorage.ValorMontoProyecto : 0);

        vm.idTipoTramite = "";
        vm.idEntidad = "1C58FFF0-E999-44C9-B4BE-0176A3CF73A5";
        vm.numeroTramite = 0;
        vm.nombreEntidad = "";
        vm.nombreTipoTramite = "";
        vm.etapa = $routeParams['etapa'];
        vm.etapa = 'ej';
        vm.tipoEntidad = 'Nacional';
        vm.nombreArchivo = '';
        var fuente = {};
        var tiporecurso = {};

        vm.ValorInicialNacion = 0;
        vm.ValorVigenteNacion = 0;
        vm.ValorDisponibleNacion = 0;
        vm.ValorVigenciasFuturasNacion = 0;
        vm.ValorInicialPropios = 0;
        vm.ValorVigentePropios = 0;
        vm.ValorDisponiblePropios = 0;
        vm.ValorVigenciasFuturasPropios = 0;

        vm.ValorTotalMontoCSFNacion = 0;
        vm.ValorTotalMontoSSFNacion = 0;
        vm.ValorTotalMontoCSFPropios = 0;
        vm.ValorTotalMontoSSFPropios = 0;
        vm.ValorTotalCSF = 0;
        vm.ValorTotalSSF = 0;
        vm.tabcdpEsActivo = tabcdpEsActivo;
        $sessionStorage.tabcdpactivo = false;


        /*variables configuación grids */
        vm.gridFuentes = {};
        vm.gridTiposRecurso = {};


        vm.scrollbars = {
            NEVER: 0,
            ALWAYS: 1,
            WHEN_NEEDED: 2
        }

        vm.montoProyectoFuenteTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">CSF</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">SSF</label> </div > ';

        vm.montoProyectoInicialTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorInicialCSF}}</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorInicialSSF}}</label> </div > ';

        vm.montoProyectoVigenteTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorVigenteCSF}}</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorVigenteSSF}}</label> </div > ';

        vm.montoProyectoDisponibleTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorDisponibleCSF}}</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorDisponibleSSF}}</label> </div > ';

        vm.montoContracreditoTemplate = '<div class="row text-right " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;" ng-value="row.entity.ValorContracreditoCSF" ng-keypress="grid.appScope.onKeyPress_st($event)" ng-blur="grid.appScope.actualizaFila($event,1)" /> </div > ' +
            '<div class="row text-right " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;" ng-value="row.entity.ValorContracreditoSSF"  ng-keypress="grid.appScope.onKeyPress_st($event)" ng-blur="grid.appScope.actualizaFila($event,2)"/> </div > ';


        /*variables definiciones grids */
        vm.columnDefFuente = [
            {
                field: "idproyectotramite",
                displayName: 'Id Proyecto fuente presupuestal',
                enableHiding: false,
                visible: false

            },
            {
                field: "idproyectofuentetramite",
                displayName: 'Id Proyecto fuente presupuestal',
                enableHiding: false,
                visible: false

            },
            {
                field: 'idfuente',
                enableHiding: false,
                visible: false
            },
            {
                field: "idTipoValorInicial",
                enableHiding: false,
                visible: false

            },
            {
                field: "idTipoValorVigente",
                enableHiding: false,
                visible: false

            },
            {
                field: "idTipoValorContracreditoCSF",
                enableHiding: false,
                visible: false

            },
            {
                field: "idTipoValorContracreditoSSF",
                enableHiding: false,
                visible: false

            },
            {
                field: 'tiporecurso',
                displayName: 'Tipo Recurso',
                enableHiding: false,
                minWidth: 200,
                cellClass: 'text-justificado',
                enableColumnMenu: false,
            },
            {
                field: 'valor',
                displayName: ' ',
                enableHiding: false,
                minWidth: 100,
                enableColumnMenu: false,
                cellTemplate: vm.montoProyectoFuenteTemplate
            },
            {
                field: 'valorinicial',
                displayName: 'Valor Inicial ',
                enableHiding: false,
                minWidth: 100,
                enableColumnMenu: false,
                cellTemplate: vm.montoProyectoInicialTemplate
            },
            {
                field: 'valorvigente',
                displayName: 'Valor Vigente',
                enableHiding: false,
                enableColumnMenu: false,
                minWidth: 100,
                enableCellEdit: true,
                cellTemplate: vm.montoProyectoVigenteTemplate

            },
            {
                field: 'valodisponible',
                displayName: 'Valor Disponible',
                enableHiding: false,
                enableColumnMenu: false,
                minWidth: 100,
                enableCellEdit: true,
                cellTemplate: vm.montoProyectoDisponibleTemplate

            },
            {
                field: 'valorcontracredito',
                displayName: 'Valor ' + vm.TipoProyecto,
                Width: 100,
                enableColumnMenu: false,
                cellTemplate: vm.montoContracreditoTemplate
                //cellTemplate: '<div><input style="text-align:right;" ng-model="row.entity.valorcontracreditoSSF" placeholder="Digite el valor" ng-keypress="grid.appScope.onKeyPress($event)" ng-blur="grid.appScope.actualizaFila($event)" style="width:70%" /></div>'
            },
            //{
            //    field: 'valorcontracreditoCSF',
            //    displayName: 'Valor-Contracredito CSF',
            //    minWidth: 100,
            //    enableColumnMenu: false,
            //    cellTemplate: '<div><input style="text-align:right;" ng-model="row.entity.valorcontracreditoCSF" placeholder="Digite el valor" ng-keypress="grid.appScope.onKeyPress($event)" ng-blur="grid.appScope.actualizaFila($event)" style="width:70%" /></div>'

            //}
        ];

        vm.columnDefTipoRecursos = [
            {
                field: "idTipoRecurso",
                displayName: 'Id ',
                visible: false
            },
            {
                field: "TipoRecurso",
                displayName: 'Tipo de recurso ',
                minWidth: 165
            },
            {
                field: "ValorSSF",
                displayName: 'SSF ',
                minWidth: 165
            },
            {
                field: "ValorCSF",
                displayName: 'CSF ',
                minWidth: 165
            }
        ];

        vm.columnDefValoresProyecto = [
            {
                field: "TipoRecurso",
                displayName: ' ',
                minWidth: 165
            },
            {
                field: "AprobacionInicial",
                displayName: 'Aprobacion Inicial ',
                minWidth: 165
            },
            {
                field: "AprobacionVigente",
                displayName: 'Aprobacion Vigente ',
                minWidth: 165
            },
            {
                field: "ValorDisponible",
                displayName: 'Valor Disponible ',
                minWidth: 165
            },
            {
                field: "VigenciasFuturas",
                displayName: 'Vigencias Futuras ',
                minWidth: 165
            }
        ];



        /*variables datos grids */
        vm.datosFuente = [];
        vm.datosTiposRecurso = [];
        vm.datosValoresProyectos = [];


        /*variables listas */
        vm.listaVigencia = {};
        vm.listaTipoRecursos = {};
        vm.listaFuentesPresuestales = [];


        /*variables metodos */
        vm.cargarFuentesInformacionPresupuestal = cargarFuentesInformacionPresupuestal;
        vm.cargarProyectoFuentesPrespuestalPorTramite = cargarProyectoFuentesPrespuestalPorTramite;
        vm.cargarValoresProyectos = cargarValoresProyectos;
        vm.addFila = addFila;
        vm.cargarGridTipoRecursos = cargarGridTipoRecursos;
        vm.retornar = retornar;
        vm.actualizarTramitesFuentes = actualizarTramitesFuentes;
        vm.tabcdpactivo = false;



        /*Carga datos en variables */
        vm.listaVigencia = {
            model: null,
            items: [
                { value: 0, name: 'Seleccione...' },
                { value: 2020, valor: '2020' },
                { value: 2021, valor: '2021' },
                { value: 2021, valor: '2022' },
                { value: 2021, valor: '2023' }
            ]
        };

        vm.datosTiposRecurso = [
            {
                idTipoRecurso: 'N',
                TipoRecurso: 'Inversión nación',
                ValorCSF: 0,
                ValorSSF: 0
            },
            {
                idTipoRecurso: 'P',
                TipoRecurso: 'Inversión propios',
                ValorCSF: 0,
                ValorSSF: 0
            },
            {
                idTipoRecurso: 'P',
                TipoRecurso: 'Totales',
                ValorCSF: 0,
                ValorSSF: 0,
                cellClass: 'col-font'
            }
        ];

      

        /*Carga grids */

        vm.gridFuentes = {
            enableCellEditOnFocus: true,
            minRowsToShow: 2,
            columnDefs: vm.columnDefFuente,
            data: vm.datosFuente
        };

        vm.gridTiposRecurso = {
            minRowsToShow: 2,
            columnDefs: vm.columnDefTipoRecursos,
            data: vm.datosTiposRecurso,
            enableVerticalScrollbar: vm.scrollbars.NEVER,
            enableHorizontalScrollbar: vm.scrollbars.WHEN_NEEDED
        };




        vm.gridFuentes.appScopeProvider = vm;
        vm.gridFuentes.onRegisterApi = function (gridApi) {
            vm.gridApi = gridApi;
        };



        /*Carga variables */





        /**Ejecucion de metodos */

        vm.inicializar = function () {
            if ($sessionStorage.accionEjecutandose === 'Ver requisitos proyecto') {
                vm.ocultarRequisitosTramite = true;
                vm.ocultarRequisitosProyecto = false;
                vm.TabActived = false;
            }
            else
                if ($sessionStorage.accionEjecutandose === 'Ver requisitos tramite') {
                    vm.ocultarRequisitosTramite = false;
                    vm.ocultarRequisitosProyecto = true;
                    vm.TabActived = true;
                }


            cargainicial().then(function (status) { cargarProyectoFuentesPrespuestalPorTramite(true); }, function (error) { });

        };

        function cargainicial() {
            var deferred = $q.defer();
            try {
                cargarFuentesInformacionPresupuestal();
                vm.cargarValoresProyectos();
                deferred.resolve("done");
            } catch (err) {

                deferred.reject(err);
            }
            return deferred.promise;
        }




        vm.getTableHeightFuentes = function () {
            var rowHeight = 30;
            var headerHeight = 30;
            var alto = (((vm.datosFuente.length + 1) * rowHeight + headerHeight) + 30).toString() + "px";
            return {
                height: alto
            };
        }

        vm.getTableHeightTiposRecurso = function () {
            var rowHeight = 30;
            var headerHeight = 50;
            return {
                height: (((vm.datosTiposRecurso.length + 1) * rowHeight + headerHeight) + 30) + "px"
            };
        }



        vm.actualizaFila = function (event, tipoSF) {
            var valor = '0';
            $(event.target).val(function (index, value) {
                valor =  formatearNumero(value === '' ? 0 : value);
            });
            const btnScope = angular.element(event.target).scope();
            const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;

            cargarGridTipoRecursos(valor, tipoSF, entity.tiporecurso);


        }

        vm.onKeyPress = function (e) {
            const charCode = e.which ? e.which : e.keyCode;

            if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                e.preventDefault();;
            }

        }
       

        function tabcdpEsActivo() {
            vm.tabcdpactivo = true;
            $sessionStorage.tabcdpactivo = vm.tabcdpactivo;
        }

        function actualizarTramitesFuentes() {
            var tramitefuentesDto = [];
            var valortotalnacion = 0;
            var valortotalpropios = 0;
            var valortotal = 0;
            //valorFuenteNacion

            vm.gridFuentes.data.map(function (item, index) {
                if (item.tiporecurso.includes('propio')) {
                    valortotalpropios += parseInt(limpiaNumero(item.ValorContracreditoCSF === '' ? 0 : item.ValorContracreditoCSF));
                    valortotalpropios += parseInt(limpiaNumero(item.ValorContracreditoSSF === '' ? 0 : item.ValorContracreditoSSF));
                    valortotal += valortotalpropios;
                }
                else {
                    valortotalnacion += parseInt(limpiaNumero(item.ValorContracreditoCSF === '' ? 0 : item.ValorContracreditoCSF));
                    valortotalnacion += parseInt(limpiaNumero(item.ValorContracreditoSSF === '' ? 0 : item.ValorContracreditoSSF));
                    valortotal += valortotalnacion;
                }

                //if (
                //    parseInt(limpiaNumero(item.ValorContracreditoCSF === '' ? 0 : item.ValorContracreditoCSF)) > parseInt(limpiaNumero(item.ValorVigenteCSF === '' ? 0 : item.ValorVigenteCSF)) ||
                //    parseInt(limpiaNumero(item.ValorContracreditoSSF === '' ? 0 : item.ValorContracreditoSSF)) > parseInt(limpiaNumero(item.ValorVigenteSSF === '' ? 0 : item.ValorVigenteSSF))
                //) {
                //    utilidades.mensajeError("El valor contracredito del tipo de recurso " + item.tiporecurso + " es mayor al valor vigente!");
                //    resultado = false;
                //}


            });
            if (!(parseInt(valortotalnacion) === parseInt(limpiaNumero(vm.valorFuenteNacion === '' || vm.valorFuenteNacion === undefined  ? 0 : vm.valorFuenteNacion)))) {
                utilidades.mensajeError("La suma de los valores a contracreditar nación en los tipos de recurso son diferentes al valor del monto nación trámite!");
                resultado = false;
            }
            if (!(parseInt(valortotalpropios) === parseInt(limpiaNumero(vm.valorFuentePropios === '' || vm.valorFuentePropios === undefined ? 0 : vm.valorFuentePropios)))) {
                utilidades.mensajeError("La suma de los valores a contracreditar propios en los tipos de recurso son diferentes al valor del monto propios trámite!");
                resultado = false;
            }

            vm.gridFuentes.data.map(function (item, index) {
                var tramiteFuentePrespuestal = {
                    IdPresupuestoValores: item.idpresupuestovalores
                    , IdFuente: item.idfuente
                    , IdProyectoFuenteTramite: item.idproyectofuentetramite
                    , IdProyectoTramite: item.idproyectotramite
                    , TipoRecurso: item.tiporecurso
                    , ValorContracreditoCSF: limpiaNumero(item.ValorContracreditoCSF)
                    , ValorContracreditoSSF: limpiaNumero(item.ValorContracreditoSSF)
                    //, ValorInicialCSF: limpiaNumero(item.valorinicialCSF)
                    //, ValorInicialSSF: limpiaNumero(item.valorinicialSSF)
                    //, ValorvigenteCSF: limpiaNumero(item.valorvigenteCSF)     
                    //, ValorvigenteSSF: limpiaNumero(item.valorvigenteSSF)
                    //, IdTipoValorInicialCSF: item.idTipoValorInicialCSF
                    //, IdTipoValorInicialSSF: item.idTipoValorInicialSSF
                    //, IdTipoValorVigenteCSF: item.idTipoValorVigenteCSF
                    //, IdTipoValorVigenteSSF: item.idTipoValorVigenteSSF
                    //, IdTipoValorDisponibleCSF: item.idTipoValorDisponibleCSF
                    //, IdTipoValorDisponibleSSF: item.idTipoValorDisponibleSSF
                    , IdTipoValorContracreditoCSF: item.idTipoValorContracreditoCSF
                    , IdTipoValorContracreditoSSF: item.idTipoValorContracreditoSSF
                    , Accion: vm.TipoProyecto == 'Credito' ? 'D' : 'C'
                    , IdProyecto: vm.ProyectoId
                    , IdTramite: vm.TramiteId
                }
                tramitefuentesDto.push(tramiteFuentePrespuestal);
            });

            trasladosServicio.actualizarTramitesFuentesPresupuestales(tramitefuentesDto).then(function (response) {
                if (response.data && (response.statusText === "OK" || response.status === 200)) {
                    parent.postMessage("cerrarModal", window.location.origin);
                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                } else {
                    swal('', "Error al realizar la operación", 'error');
                }
            });

        }


        function cargarValoresProyectos() {
            //var tramites = trasladosServicio.obtenerTarmitesEstadoCerrado();
            var entidadtmp = vm.EntidadId === undefined ? 0 : vm.EntidadId;
            var tr = trasladosServicio.obtenerValoresProyectos(vm.TramiteId, vm.ProyectoId, entidadtmp)
                .then(function (result) {
                    if (result != null) {

                        vm.ValorInicialNacion = formatearNumero(result.data.DecretoNacion);
                        vm.ValorVigenteNacion = formatearNumero(result.data.VigenteNacion);
                        vm.ValorDisponibleNacion =   formatearNumero(result.data.DisponibleNacion);
                        vm.ValorVigenciasFuturasNacion = formatearNumero(result.data.VigenciaFuturaNacion);

                        vm.ValorInicialPropios = formatearNumero(result.data.DecretoPropios);
                        vm.ValorVigentePropios = formatearNumero(result.data.VigentePropios);
                        vm.ValorDisponiblePropios =   formatearNumero(result.data.DisponiblePropios);
                        vm.ValorVigenciasFuturasPropios = formatearNumero(result.data.VigenciaFuturaPropios);


                    }
                });
        }


        function cargarFuentesInformacionPresupuestal() {
            var items = [];
            var tr = trasladosServicio.obtenerFuentesInformacionPresupuestal().then(function (result) {
                vm.listaFuentesPresuestales = result.data;
                if (result.data !== undefined && result.data.length > 0) {
                    result.data.map(function (item, index) {
                        var nombre = item.Origen === 'N' ? item.Nombre + '-nación' : item.Nombre + '-propios';
                        items.push({ id: item.Id, valor: nombre });
                    });
                }
                vm.listaTipoRecursos = {
                    model: null,
                    items: items
                };
                return true;
            });
        }



        function cargarProyectoFuentesPrespuestalPorTramite(boolConsulta) {
            limpiaFuente();
            var tr = trasladosServicio.obtenerProyectoFuentePresupuestalPorTramite(vm.ProyectoId, vm.TramiteId, vm.TipoProyecto).then(function (result) {
                if (boolConsulta && result !== undefined && result.data.length > 0)
                    AgregarFuentesPrespuestalPorTramite(result);
                else if (vm.idFuentePresupuestal > 0) {
                    limpiaFuente();
                    var item = vm.listaTipoRecursos.items.filter(x => x.id.toString() === vm.idFuentePresupuestal.toString());
                    fuente.idproyectotramite = vm.idProyectoTramite;
                    fuente.idfuente = vm.idFuentePresupuestal;
                    fuente.tiporecurso = item[0].valor;
                    fuente.valorinicialCSF = '';
                    fuente.tiporecurso = fuenteTmp
                    fuente.ValorInicialCSF = 0;
                    fuente.ValorInicialSSF = 0;
                    fuente.ValorVigenteCSF = 0;
                    fuente.ValorVigenteSSF = 0;
                    fuente.ValorDisponibleCSF = 0;
                    fuente.ValorDisponibleSSF = 0;
                    fuente.ValorContracreditoCSF = 0;
                    fuente.ValorContracreditoSSF = 0;
                    fuente.IdValorContracreditoSSF = 0;
                    fuente.IdValorContracreditoCSF = 0;
                    vm.datosFuente.push(fuente);
                }
            }).then(function (result) { cargarGridTipoRecursos(0,0,0); });

        }


        function AgregarFuentesPrespuestalPorTramite(result) {
            fuente.idproyectofuentetramite = 0;
            if (result.data !== undefined && result.data.length > 0) {
                result.data.map(function (item, index) {
                    //if (fuente.idproyectofuentetramite > 0)
                    //    vm.datosFuente.push(fuente);
                    limpiaFuente();
                    fuente.idproyectofuentetramite = item.Id;
                    fuente.idproyectotramite = item.TramiteProyectoId;

                    if (item.ListaFuentes !== undefined && item.ListaFuentes.length > 0) {
                        item.ListaFuentes.map(function (itemfuente, indexfuente) {
                            fuente.idfuente = itemfuente.Id;
                            var fuenteTmp = vm.listaFuentesPresuestales.filter(x => x.Id.toString() === fuente.idfuente.toString());
                            fuente.tiporecurso = fuenteTmp[0].Origen === 'N' ? fuenteTmp[0].Nombre + '-nación' : fuenteTmp[0].Nombre + '-propios';

                            fuente.ValorInicialSSF = formatearNumero(itemfuente.ValorInicialSSF);
                            fuente.ValorInicialCSF = formatearNumero(itemfuente.ValorInicialCSF);
                            fuente.ValorVigenteSSF = formatearNumero(itemfuente.ValorVigenteSSF);
                            fuente.ValorVigenteCSF = formatearNumero(itemfuente.ValorVigenteCSF);
                            fuente.ValorDisponibleSSF = formatearNumero(itemfuente.ValorDisponibleSSF);
                            fuente.ValorDisponibleCSF = formatearNumero(itemfuente.ValorDisponibleCSF);
                            fuente.ValorContracreditoSSF = formatearNumero(itemfuente.ValorContracreditoSSF);
                            fuente.ValorContracreditoCSF = formatearNumero(itemfuente.ValorContracreditoCSF);
                            fuente.idTipoValorContracreditoSSF = itemfuente.idTipoValorContracreditoSSF;
                            fuente.idTipoValorContracreditoCSF = itemfuente.idTipoValorContracreditoCSF;
                        });

                    }
                    vm.datosFuente.push(fuente);
                });
            }
            //vm.datosFuente.push(fuente);
        }



        function cargarGridTipoRecursos(valor, tipoSF, tiporecurso) {
            $timeout(function () {
                $scope.$apply(function () {
                   
                    vm.ValorTotalMontoCSFNacion = 0;
                    vm.ValorTotalMontoSSFNacion = 0;
                    vm.ValorTotalMontoCSFPropios = 0;
                    vm.ValorTotalMontoSSFPropios = 0;
                    vm.ValorTotalCSF = 0;
                    vm.ValorTotalSSF = 0;

                    var valortotalSSf = 0;
                    var valortotalCSf = 0;
                    vm.datosFuente.map(function (item, index) {
                        if (tiporecurso === item.tiporecurso) {
                            if (tipoSF === 2) {
                                item.ValorContracreditoSSF = valor;
                            }
                            else if (tipoSF === 1) {
                                item.ValorContracreditoCSF = valor;
                            }
                        }
                        var tr = vm.listaFuentesPresuestales.filter(x => x.Id.toString() === item.idfuente.toString());
                        if (tr.length > 0) {
                            var valorSSF = parseInt(limpiaNumero(item.ValorContracreditoSSF === '' || item.ValorContracreditoSSF === undefined ? 0 : item.ValorContracreditoSSF));
                            var valorCSF = parseInt(limpiaNumero(item.ValorContracreditoCSF === '' || item.ValorContracreditoCSF === undefined ? 0 : item.ValorContracreditoCSF));
                            if (tr[0].Origen === 'P') {
                                vm.ValorTotalMontoCSFPropios = formatearNumero(parseInt(limpiaNumero(vm.ValorTotalMontoCSFPropios === '' || vm.ValorTotalMontoCSFPropios === undefined ? 0 : vm.ValorTotalMontoCSFPropios)) + parseInt(valorCSF));
                                vm.ValorTotalMontoSSFPropios = formatearNumero(parseInt(limpiaNumero(vm.ValorTotalMontoSSFPropios === '' || vm.ValorTotalMontoSSFPropios === undefined ? 0 : vm.ValorTotalMontoSSFPropios)) + parseInt(valorSSF));
                                valortotalSSf += parseInt(valorSSF);
                                valortotalCSf += parseInt(valorCSF);
                            }
                            else if (tr[0].Origen === 'N') {
                                vm.ValorTotalMontoCSFNacion = formatearNumero(parseInt(limpiaNumero(vm.ValorTotalMontoCSFNacion === '' || vm.ValorTotalMontoCSFNacion === undefined ? 0 : vm.ValorTotalMontoCSFNacion)) + parseInt(valorCSF));
                                vm.ValorTotalMontoSSFNacion = formatearNumero(parseInt(limpiaNumero(vm.ValorTotalMontoSSFNacion === '' || vm.ValorTotalMontoSSFNacion === undefined ? 0 : vm.ValorTotalMontoSSFNacion)) + parseInt(valorSSF));
                                valortotalSSf += parseInt(valorSSF);
                                valortotalCSf += parseInt(valorCSF);
                            }
                            vm.ValorTotalCSF = formatearNumero(valortotalCSf);
                            vm.ValorTotalSSF = formatearNumero(valortotalSSf);
                        }

                    });
                });
            });

        }

        function ValidaSiEsNumero(valor) {
            if (valor === undefined)
                return false;
            else if (!isNaN(limpiaNumero(valor))) {
                return true;
            }
            else {
                return false;
            }

        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        function cambiarComasPorPunto(valor) {
            return valor.toString().replaceAll(",", ".");
        }

        function formatearNumero(value) {
            var numerotmp = value.toString().replaceAll('.', '');
            return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }

        function formatoFecha(value) {
            value = new Date(value);
            var dia = value.getDate() + "";
            var mes = (value.getMonth() + 1) + "";
            var ano = value.getFullYear() + "";
            return ano + "-" + agregarCero(mes) + "-" + agregarCero(dia);
        }

        function agregarCero(value) {
            if (value.length == 1) {
                value = "0" + value;
            }
            return value;

        }

        function limpiaFuente() {
            fuente = {
                idproyectotramite: '',
                idproyectofuentetramite: '',
                idpresupuestovalores: '',
                tiporecurso: '',
                valorinicialSSF: '',
                valorinicialCSF: '',
                valorvigenteSSF: '',
                valordisponibleCSF: '',
                valordisponibleSSF: '',
                valorcontracreditoCSF: '',
                valorcontracreditoSSF: '',
                idTipoValorContracreditoSSF: '',
                idTipoValorContracreditoCSF: ''
            }
        }

        function addFila(tiporecursoseleccionado) {
            vm.idFuentePresupuestal = tiporecursoseleccionado.model;

            $timeout(function () {
                $scope.$apply(function () {
                    cargarProyectoFuentesPrespuestalPorTramite(false);
                });
            });

        }

        function retornar() {
            //$timeout(function () {
            //    $location.path('/tramiteTraslados');
            //}, 300);
            vm.desactivartraslados();
        }

       

       



    }


    // angular.module('backbone').controller('trasladosAccionesController', trasladosAccionesController)
    angular.module('backbone').component('trasladosAcciones', {
        templateUrl: "src/app/formulario/ventanas/tramites/trasladosAcciones.html",
        controller: trasladosAccionesController,
        controllerAs: "vm",
        bindings: {
            desactivartraslados: '&'
        }
    });



})();