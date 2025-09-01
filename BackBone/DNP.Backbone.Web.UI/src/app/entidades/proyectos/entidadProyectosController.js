var entidadProyectosCtrl = null;

try {
    (function(){
        let module = angular.module('backbone.entidades');

        /**
         * 
         * @description entidadProyectosController. Controlador específico de la pantalla Entidades/Proyectos
         * @param {object} $scope. Scope actual. Angular js
         * @param {Function} servicioEntidadProyectos. Servicios, funciones y operaciones request específicas para éste modulo 
         * @param {Function} constantesBackbone . Conjunto de constantes de la aplicaciòn
         */
        let entidadProyectosController = function($scope, servicioEntidadProyectos, constantesBackbone, $uibModal, constantesTipoFiltro, $localStorage){

            entidadProyectosCtrl = this;

            entidadProyectosCtrl.$scope = $scope;
            
            entidadProyectosCtrl.plantillas = {
                proyectoGrupoTemplate   : 'src/app/entidades/proyectos/plantillas/proyectoGrupoTemplate.html',
                proyectoSubgrupoTemplate: 'src/app/entidades/proyectos/plantillas/proyectoSubgrupoTemplate.html',
                proyectoCellTemplate    : 'src/app/entidades/proyectos/plantillas/proyectoCellTemplate.html',
                estadoCellTemplate      : 'src/app/entidades/proyectos/plantillas/estadoCellTemplate.html',
                flujoEstadoTemplate     : 'src/app/entidades/proyectos/plantillas/proyectoFlujoCellTemplate.html',
                accionesTemplate        : 'src/app/entidades/proyectos/plantillas/accionesCellTemplate.html',

                // determina de forma estatica los colores de los estados del proyecto
                colorEstados : {
                    info      : 'info',
                    completado: 'success',
                    alerta    : 'warning',  
                    error     : 'danger'
                }
            };

            entidadProyectosCtrl.columnas = {

                columnDefsGrupo : [
                    {
                        field: 'GrupoNombre',
                        displayName: '',
                        enableHiding: false,
                        cellTemplate: entidadProyectosCtrl.plantillas.proyectoGrupoTemplate,
                        width:'100%',
                    },
                ],

                columnDefsSubgrupo: [
                    {
                        field: 'Descripcion',
                        displayName: 'Poyecto/BPIN',
                        enableHiding: false,
                        cellTemplate: entidadProyectosCtrl.plantillas.proyectoCellTemplate,
                        width       : '40%',
                    },
                    {
                        field: 'Estado',
                        displayName: 'Estado',
                        enableHiding: false,
                        cellTemplate: entidadProyectosCtrl.plantillas.estadoCellTemplate,
                        width: '10%'
                    },
                    {
                        field: 'FlujoDescripcion',
                        displayName: 'Flujo',
                        enableHiding: false,
                        cellTemplate: entidadProyectosCtrl.plantillas.flujoEstadoTemplate,
                        width:'25%'
                    },
                    {
                        field: 'FechaProyecto',
                        displayName: 'Fecha',
                        enableHiding: false,
                        type: 'date',
                        cellFilter: 'date:\'dd/MM/yyyy HH:mm\'',
                        width:'15%'
                    },
                    {
                         field: 'accion',
                         displayName: 'Acción',
                         headerCellClass: 'text-center',
                         enableFiltering: false,
                         enableHiding: false,
                         enableSorting: false,
                         enableColumnMenu: false,
                         cellTemplate: entidadProyectosCtrl.plantillas.accionesTemplate,
                         width: '10%'
                     },
                 ],

                 indiceFiltros: [
                     true, // entidad, 
                     true, // proyecto,
                     true, // estado del proyecto
                     true, // fecha del proyecto
                 ]
            },

            entidadProyectosCtrl.grid = {

                proyectoGridApi : {},
                proyectoSubGridApi: {},

                // columnas: 

                // configuraciones grid agrupador
                proyectoOptions: {
                    enableSorting: true,

                    // columnas
                    columnDefs: entidadProyectosCtrl.columnas.columnDefsGrupo,

                      // expandable grid
                      expandableRowTemplate: entidadProyectosCtrl.plantillas.proyectoSubgrupoTemplate,
                      expandableRowScope: $scope.proyectoSubGridApi,

                      enableOnDblClickExpand: false,
                      enableColumnResizing: false,
                      showGridFooter: false,
                      enablePaginationControls: true,
                      useExternalPagination: false,
                      useExternalSorting: false,
                      paginationCurrentPage: 1,
                      enableVerticalScrollbar: 1,
                      enableFiltering: false,
                      showHeader: false,
                      useExternalFiltering: false,
                      paginationPageSizes: [5, 10, 15, 25, 50, 100],
                      paginationPageSize: 10,

                      // Registrar API
                      onRegisterApi: (gridApi) => {
                          
                        entidadProyectosCtrl.grid.proyectoGridApi = gridApi;
                      } 
                },

                /**
                 * @description Configuración del grid como subgrupo del grid principal
                 * @param {Array} data . Arreglo de objetos que conforman los datos del subgrupo del grid principal
                 * @returns {object}. Devuelve un objeto con las configuraciones del subgrupo del grid principal
                 */
                proyectoSubgroupOptions: function(data) {
                    return {
                        // columnas
                        columnDefs: entidadProyectosCtrl.columnas.columnDefsSubgrupo,

                        // otras configuraciones
                        appScopeProvider: $scope,
                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                        paginationPageSize: 10,
                        disableRowExpandable: data.length == 0,

                        data : data,
                    };
                }
            };

            // variables de control
            entidadProyectosCtrl.control = {

                entidadClaveSeleccionada : '',

                filtros : {

                    entidadLista: [],
                    proyectoLista: [],
                    estadoProyectoLista: [],
                    mostrarPanel : false,
                },

                filtrados: true,  
                
                // utilizar para las tabs de entidades
                tabLista: [],

                // columnas de defecto de la grid de datos
                columnasDefecto : servicioEntidadProyectos.columnasDefecto
            };

            // variables de accesos por usuario
            entidadProyectosCtrl.acceso = {
                configuraciones: []
            }

            // modelo de datos para el formulario de la pantalla actual
            entidadProyectosCtrl.modelo = {

                filtro : {

                    entidadId: null,
                    proyectoId: null,
                    estadoProyectoId: null,
                    fechaInicio : null,
                    fechaTermino : null
                },

                datos : {
                    proyectoEntidadLista: []
                }
            };

            // métodos y funciones especiales
            entidadProyectosCtrl.funciones = {

                /**
                 * 
                 * @description Realiza la petición para obtener los proyectos del tipo de entidad actual, y filtra por proyecto, entidad, 
                 * @param {Number} entidadId. Identificador de la entidad actual
                 * @param {Number} proyectoId . Identificador del proyecto seleccionado
                 * @param {Number} estadoProyectoId . Identificador del estado del proyecto seleccionado
                 * @param {Date} fechaInicio. Fecha inicio seleccionada para el rango a filtrar
                 * @param {Date} fechaTermino . Fecha termino seleccionada para filtrar por rango de fechas
                 */
                filtrarProyectosEntidades : function(entidadId, proyectoId, estadoProyectoId, fechaInicio, fechaTermino){
                    try {

                        entidadId         = Number(entidadId === undefined || entidadId === null ? 0 : entidadId);
                        estadoProyectoId  = Number(estadoProyectoId === undefined || estadoProyectoId === null ? 0 : estadoProyectoId);
                        proyectoId        = Number(proyectoId === undefined || proyectoId === null ? 0 : proyectoId);
                        fechaInicio       = fechaInicio === undefined ? null : fechaInicio;
                        fechaTermino      = fechaTermino === undefined ? null : fechaTermino;

                        let entidadNombre  = entidadProyectosCtrl.control.filtros.entidadLista.filter(p => p.Id === entidadId).map(p => p.Nombre);
                        entidadNombre      = String(entidadNombre.length > 0 ? entidadNombre[0] : '');

                        // if(entidadProyectosCtrl.modelo.datos.proyectoEntidadLista.length > 0){

                            servicioEntidadProyectos.obtenerProyectoEntidad(
                                String(entidadProyectosCtrl.control.entidadClaveSeleccionada)
                            ).then(response => {
                              
                                // TODO: Obtener proyectos de la entidad seleccionada desde el servicio
                                entidadProyectosCtrl.modelo.datos.proyectoEntidadLista = response.data.filter(p => p.Datos.filter(q => q.TipoEntidadClave ===  String(entidadProyectosCtrl.control.entidadClaveSeleccionada)).length > 0 );

                                // obtener todos los grupos
                                let subGrupoLista = []; 
                                subGrupoLista     = entidadProyectosCtrl.modelo.datos.proyectoEntidadLista.length > 0 ? entidadProyectosCtrl.modelo.datos.proyectoEntidadLista.filter(p => p.Datos.length > 0).map(p => p.Datos) : [];
                                subGrupoLista     = subGrupoLista.length > 0 ? subGrupoLista.reduce( (a,b) => a.concat(b) ).map( (p,index) => { p.FechaProyecto = new Date(2020, 9, (index + 1) ); return p;}) : [];

                                // filtrar listas
                                subGrupoLista     = subGrupoLista.filter(p => 

                                        p.EntidadId === Number( entidadId === 0 ? p.EntidadId : entidadId ) &&
                                        p.EstadoId  === Number( estadoProyectoId === 0 ? p.EstadoId :  estadoProyectoId) &&
                                        p.ProyectoId === Number( proyectoId === 0 ? p.ProyectoId : proyectoId ) &&

                                        // rango de fechas
                                        (p.FechaProyecto >= new Date( fechaInicio === null ? p.FechaProyecto : fechaInicio ) && p.FechaProyecto <= new Date( fechaTermino === null ? p.FechaProyecto : fechaTermino ) )
                                    );

                                entidadProyectosCtrl.modelo.datos.proyectoEntidadLista = _.filter(entidadProyectosCtrl.modelo.datos.proyectoEntidadLista, p => p.Datos.filter(p => subGrupoLista.includes(p)).length > 0 );
                                entidadProyectosCtrl.modelo.datos.proyectoEntidadLista = entidadProyectosCtrl.modelo.datos.proyectoEntidadLista.map(p => {

                                    p.Datos = subGrupoLista.filter(q => q.EstadoId === p.GrupoId).map( (q,index) => { q.FechaProyecto = new Date(2020, 9, (index + 1));  return q;});
                                    return p;
                                });

                                // enlazar lista a la tabla
                                entidadProyectosCtrl.grid.proyectoOptions.data = entidadProyectosCtrl.modelo.datos.proyectoEntidadLista;
                                entidadProyectosCtrl.grid.proyectoOptions.data.forEach(p => {
                                    p.subGroupOptions = entidadProyectosCtrl.grid.proyectoSubgroupOptions(p.Datos);
                                });

                                entidadProyectosCtrl.control.filtrados = Boolean(subGrupoLista.length > 0);

                                if(entidadProyectosCtrl.control.filtrados === true)
                                 {  
                                     entidadProyectosCtrl.grid.proyectoGridApi.core.refresh();
                                     const {columnasDisponibles} = ($localStorage.tipoFiltro && $localStorage.tipoFiltro.entidadProyectos) ?  $localStorage.tipoFiltro.entidadProyectos : [];
                                     if(columnasDisponibles.length > 0) entidadProyectosCtrl.funciones.agregarRemoverColumnas( columnasDisponibles)
                                 }
                            });
                        // }
                    }
                    catch(exception){
                        console.log('entidadProyectosCtrl', exception);
                        alert('Ocurrió un error al filtrar los datos');
                    }
                },

                /**
                 * @description. Muestra una ventana modal para visualizar un listado seleccionable de columnas
                 *               a mostrar en grid
                 * @param {Array} columnas . Columnas existentes en el grid
                 * @param {Array} columnasPorAgregar . Columnas disponibles, que no están en el grid
                 */
                abrirModalColumnas: function(columnas, columnasPorAgregar){
                    try {
                        
                        let modal = $uibModal.open({
                            animation: true,
                            templateUrl: 'src/app/configurarColumnas/plantillaConfigurarColumnas.html',
                            controller: 'controladorConfigurarColumnas',
                            controllerAs: "vm",
                            size: 'lg',
                            resolve: {
                                items: () => ({
                                            columnasActivas: columnas,
                                            columnasDisponibles: columnasPorAgregar,
                                            tipoFiltro: constantesTipoFiltro.entidadProyecto 
                                        })
                            }
                        });

                        modal.result.then( entidadProyectosCtrl.eventos.modal_onResultThen, entidadProyectosCtrl.eventos.modal_onErrorResultThen);
                    }
                    catch(exception){
                        console.log('entidadProyectoCtrl.abrirModalColumnas => ', exception);
                        alert('Ocurrió un error al mostrar la ventana de seleccion de columnas');
                    }
                },

                /**
                 * 
                 * @description   Remueve las columnas enviadas como parametro del grid de datos
                 * @param {Array} columnas. Columnas a eliminar de las columnas por defecto y del grid
                 */
                agregarRemoverColumnas : (columnas) => {
                   
                    // obtener las columnas que no se van a remover
                    let columnasSinRemover = []; 
                    columnasSinRemover     = entidadProyectosCtrl.control.columnasDefecto.map((p, index) => ({ value: p, index: index })).filter(p =>  !columnas.includes(p.value) );
                    columnasSinRemover     = columnasSinRemover .length > 0 ? columnasSinRemover.map(p => p.index) : [];

                    // obtener las columnas del grid
                    let columnasDef = entidadProyectosCtrl.columnas.columnDefsSubgrupo.filter ((p,index) => columnasSinRemover.includes(index) || index === 4);
                    // setear las columnas al subgrupo
                    entidadProyectosCtrl.grid.proyectoOptions.data = entidadProyectosCtrl.grid.proyectoOptions.data.map(p => {

                        p.subGroupOptions.columnDefs = columnasDef;
                        return p;
                    });

                    entidadProyectosCtrl.columnas.indiceFiltros = entidadProyectosCtrl.columnas.indiceFiltros.map((p, index) => columnasSinRemover.includes(index) );


                    entidadProyectosCtrl.grid.proyectoGridApi.core.refresh();
                }
            };

            // eventos
            entidadProyectosCtrl.eventos = {
                
                /**
                 * 
                 * @description Inicializa la vista que tiene  enlazado el controlador actual 
                 */
                init : function(){
                    try {

                        entidadProyectosCtrl.control.tabLista = servicioEntidadProyectos.obtenerTiposEntidad();
                        if(entidadProyectosCtrl.control.tabLista.length > 0)
                            entidadProyectosCtrl.control.entidadClaveSeleccionada = String(entidadProyectosCtrl.control.tabLista[0].clave);

                           // obtener las entidades por roles de usuario
                        servicioEntidadProyectos.obtenerEntidades().then(data  => {
                            entidadProyectosCtrl.control.filtros.entidadLista = Array.isArray(data) ? 
                                                                                    data
                                                                                        .map(p => ({ Id: p.IdEntidad, Nombre: p.NombreEntidad, TipoEntidad: p.TipoEntidad }))
                                                                                        .filter(p => p.TipoEntidad === String(entidadProyectosCtrl.control.entidadClaveSeleccionada)) : 
                                                                                    [];

                            entidadProyectosCtrl.control.filtros.entidadLista = _.sortBy(entidadProyectosCtrl.control.filtros.entidadLista, p => p.Nombre);
                        });

                        // obtener los accesos del usuario actual
                        servicioEntidadProyectos.obtenerAccesoRol().then(data => {
                            entidadProyectosCtrl.acceso.configuraciones = data;

                            entidadProyectosCtrl.control.tabLista = entidadProyectosCtrl.control.tabLista.map(p => ({
                                         clave      : p.clave,
                                         nombre     : p.nombre, 
                                         habilitado : Boolean(entidadProyectosCtrl.acceso.configuraciones.filter(q => q.TipoEntidad === p.nombre).length > 0 ? true : false)
                             }));
                        });

                        // cargar filtros
                        servicioEntidadProyectos.obtenerEstadosProyecto().then(response => {
                            entidadProyectosCtrl.control.filtros.estadoProyectoLista  = response.data;
                        });

                        if(entidadProyectosCtrl.control.tabLista.length > 0)
                        {    
                            servicioEntidadProyectos.obtenerProyectoEntidad(
                                String(entidadProyectosCtrl.control.entidadClaveSeleccionada)
                            ).then(response => {
                              
                                // TODO: Obtener proyectos de la entidad seleccionada desde el servicio
                                entidadProyectosCtrl.modelo.datos.proyectoEntidadLista = response.data.filter(p => p.Datos.filter(q => q.TipoEntidadClave ===  String(entidadProyectosCtrl.control.entidadClaveSeleccionada)).length > 0 );

                                // enlazar lista a la tabla
                                entidadProyectosCtrl.grid.proyectoOptions.data = entidadProyectosCtrl.modelo.datos.proyectoEntidadLista;
                                // establecer los subgrupos
                                entidadProyectosCtrl.grid.proyectoOptions.data.forEach(grupoOption => {
                                    grupoOption.subGroupOptions = entidadProyectosCtrl.grid.proyectoSubgroupOptions(

                                        //TODO: Estos datos ficticios, deben ser sutituidos por datos tipo fecha reales .NET
                                        grupoOption.Datos.map( (p,index) => { p.FechaProyecto = new Date(2020, 9, Number(index  + 1));  return p; })
                                    );
                                });

                                // TODO: Obtener la lista de proyectos existentes
                                entidadProyectosCtrl.control.filtros.proyectoLista     = response.data.map(p => p.Datos).reduce((a,b) => a.concat(b)).map(p => ({ Id: p.ProyectoId, Descripcion: p.Descripcion }));
                                
                                // inicializar columnas visibles
                                const {columnasDisponibles} = ($localStorage.tipoFiltro && $localStorage.tipoFiltro.entidadProyectos) ?  $localStorage.tipoFiltro.entidadProyectos : [];
                                if(columnasDisponibles.length > 0) entidadProyectosCtrl.funciones.agregarRemoverColumnas( columnasDisponibles)
                            });
                        }
                    }
                    catch(exception){
                        console.log('entidadProyectosCtrl: ', exception);
                        alert('Ocurrió un error al inicializar la página');
                    }
                },

                /**
                 * 
                 * @description Provocado al presionar el botón del filtro
                 * @param {Event} $event . Evento provocado por el componente HTML origen
                 */
                btnFiltro_onClick : function ($event){
                    try {
                        entidadProyectosCtrl.control.filtros.mostrarPanel = !entidadProyectosCtrl.control.filtros.mostrarPanel;
                    }
                    catch(exception){
                        console.log('entidadProyectosCtrl.btnFiltro_onClick => ', exception);
                        alert('Ocurrió un error al mostrar los filtros');
                    }
                },

                /**
                 * 
                 * @description Provocado al seleccionar una tab de tipos de entidades
                 * @param {Event} $event . Evento provocado por el componente HTML origen
                 * @param {object} sender. Componente HTML origen que provocó el evento
                 */
                navTab_onClick: function($event, sender){
                    try {
                        
                        var scopeActual = angular.element(sender).parent().scope();
                        entidadProyectosCtrl.control.entidadClaveSeleccionada = String((scopeActual !== null && scopeActual !== undefined) ? scopeActual.tipo.clave : entidadProyectosCtrl.control.entidadClaveSeleccionada);

                        if(scopeActual !== null || scopeActual !== undefined){

                            // obtener las entidades por roles de usuario
                            servicioEntidadProyectos.obtenerEntidades().then(data  => {
                                entidadProyectosCtrl.control.filtros.entidadLista = Array.isArray(data) ? 
                                                                                    data
                                                                                        .map(p => ({ Id: p.IdEntidad, Nombre: p.NombreEntidad, TipoEntidad: p.TipoEntidad }))
                                                                                        .filter(p => p.TipoEntidad === String(entidadProyectosCtrl.control.entidadClaveSeleccionada)) : 
                                                                                    [];
                                entidadProyectosCtrl.control.filtros.entidadLista = _.sortBy(entidadProyectosCtrl.control.filtros.entidadLista, p => p.GrupoNombre);
                            });

                             // realizar filtro por entidad
                            servicioEntidadProyectos.obtenerProyectoEntidad(
                                String(entidadProyectosCtrl.control.entidadClaveSeleccionada)
                            ).then(response => {
                            
                                // TODO: Obtener proyectos de la entidad seleccionada desde el servicio
                                entidadProyectosCtrl.modelo.datos.proyectoEntidadLista = response.data.filter(p => p.Datos.filter(q => q.TipoEntidadClave ===  String(entidadProyectosCtrl.control.entidadClaveSeleccionada)).length > 0 );

                                // enlazar lista a la tabla
                                entidadProyectosCtrl.grid.proyectoOptions.data = entidadProyectosCtrl.modelo.datos.proyectoEntidadLista;
                                // establecer los subgrupos
                                entidadProyectosCtrl.grid.proyectoOptions.data.forEach(grupoOption => {
                                    grupoOption.subGroupOptions = entidadProyectosCtrl.grid.proyectoSubgroupOptions(

                                        //TODO: Estos datos ficticios, deben ser sutituidos por datos tipo fecha reales .NET
                                        grupoOption.Datos.map( (p,index) => { p.FechaProyecto = new Date(2020, 9, Number(index  + 1));  return p; })
                                    );
                                });

                                // TODO: Obtener la lista de proyectos existentes
                                entidadProyectosCtrl.control.filtros.proyectoLista     = response.data.map(p => p.Datos).reduce((a,b) => a.concat(b)).map(p => ({ Id: p.ProyectoId, Descripcion: p.Descripcion }));

                                // inicializar columnas visibles
                                const {columnasDisponibles} = ($localStorage.tipoFiltro && $localStorage.tipoFiltro.entidadProyectos) ?  $localStorage.tipoFiltro.entidadProyectos : [];
                                if(columnasDisponibles.length > 0) entidadProyectosCtrl.funciones.agregarRemoverColumnas( columnasDisponibles)
                            });
                        }
                    }
                    catch(exception){
                        console.log('entidadProyectosCtrl.navTab_onClick => ', exception);
                        alert('Ocurrió un error al mostrar los filtros');
                    }
                },

                /**
                 * 
                 * @description Provocado al presionar el botón "buscar", para filtrar los datos
                 * @param {Event} $event. Evento provocado por el componente HTML origen
                 * @param {object} sender . Componente HTML origen que provocó el evento
                 */
                btnAccionFiltrar_onClick: function($event, sender){
                    try {
                        entidadProyectosCtrl.funciones.filtrarProyectosEntidades(
                            entidadProyectosCtrl.modelo.filtro.entidadId,
                            entidadProyectosCtrl.modelo.filtro.proyectoId,
                            entidadProyectosCtrl.modelo.filtro.estadoProyectoId,
                            entidadProyectosCtrl.modelo.filtro.fechaInicio,
                            entidadProyectosCtrl.modelo.filtro.fechaTermino
                        );
                    }
                    catch(exception){
                        console.log('entidadProyectosCtr.btnAccionFiltrar => ', console.log);
                        alert('Ocurrió un error al filtrar los datos');
                    }
                },

                  /**
                 * 
                 * @description Provocado al presionar el botón "limpiar", para limpiar los datos seleccionados del filtro
                 * @param {Event} $event. Evento provocado por el componente HTML origen
                 * @param {object} sender . Componente HTML origen que provocó el evento
                 */
                btnLimpiarFiltro_onClick: function($eve, sender){
                    try {

                        entidadProyectosCtrl.funciones.filtrarProyectosEntidades(
                            null,
                            null,
                            null,
                            null,
                            null
                        );
                    }
                    catch(exception){
                        console.log('entidadProyectosCtr.btnLimpiarFiltro_onClick => ', console.log);
                        alert('Ocurrió un error al limpiar los filtros');
                    }
                },

                /**
                 * 
                 * @description . Provocado al presionar el botón para añadir una nueva columna
                 * @param {Event} $event . Evento provacod por el componente HTML Origen
                 * @param {object} sender. Componente HTML que provoca el evento
                 */
                btnAgregaColumna_onClick: function($event, sender) {
                    try {
                        // obtener del storage las columnas disponibles y actuales
                        let {columnasActivas, columnasDisponibles} = { columnasActivas: [], columnasDisponibles: []};
                        if($localStorage.tipoFiltro && $localStorage.tipoFiltro.entidadProyectos)
                        { 
                            columnasActivas     = $localStorage.tipoFiltro.entidadProyectos.columnasActivas;
                            columnasDisponibles = $localStorage.tipoFiltro.entidadProyectos.columnasDisponibles;
                        }
                        else
                            // obtener de las columnas enlazadas al grid
                            columnasDisponibles = entidadProyectosCtrl.control.columnasDefecto;

                        entidadProyectosCtrl.funciones.abrirModalColumnas(columnasActivas, columnasDisponibles);

                    }
                    catch(exception){
                        console.log('entidadProyectosCtr.btnAgregaColumna_onClick => ', exception);
                        alert('Ocurrió un error al añadir la columna');
                    }
                },

                /**
                 * 
                 * @description Evento provocado al terminar la promesa del resultado, del modal, al seleccionar
                 *              un elemento
                 * @param {Object} data . Objeto que contiene las lista de elementos seleciconados y disponibles
                 */
                modal_onResultThen: function( data ){
                    try {
                        
                        // obtener la lista de columnas seleccionadas, y disponibles
                        const {columnasActivas, columnasDisponibles} = data;

                        if (!$localStorage.tipoFiltro) {

                            $localStorage.tipoFiltro = {
                                entidadProyectos: {
                                    columnasActivas: columnasActivas,
                                    columnasDisponibles: columnasDisponibles
                                }
                            };
                        } else {
                            $localStorage.tipoFiltro['entidadProyectos'] = {
                                columnasActivas: columnasActivas,
                                columnasDisponibles: columnasDisponibles
                            }
                        }

                        entidadProyectosCtrl.funciones.agregarRemoverColumnas(columnasDisponibles);
                    }
                    catch(exception){
                        console.log('entidadProyectosCtrl.modal_onResultThen => ', exception);
                        alert('Ocurriò un error al realizar ésta operación');
                    }
                },

                modal_onErrorResultThen: function() {
                    console.log(`Modal dismissed at: ${new Date()}`)
                }
            };
        }

        entidadProyectosController.$inject = [
            '$scope',
            'servicioEntidadProyectos',
            'constantesBackbone',
            '$uibModal',
            'constantesTipoFiltro',
            '$localStorage'
        ];

        module.controller('entidadProyectosController', entidadProyectosController);
    })();
}
catch(exception) {
    console.log('entidadProyectosController: ', exception);
    alert('Error al inicializar el controlador. EntidadProyectos');
}