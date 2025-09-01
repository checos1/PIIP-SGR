(function () {
    'use strict';

    controladorTabla.$inject = ['$timeout'];

    function controladorTabla($timeout) {
        var vm = this;

        vm.plantillaAccionesTabla = 'src/app/panelPrincial/test/plantillaAccionesTabla.html';

        vm.columnDef = [{
                field: 'ProyectoId',
                displayName: 'Identificador',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'IdObjetoNegocio',
                displayName: 'BPIN',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'NombreObjetoNegocio',
                displayName: 'Nombre',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'NombreEntidad',
                displayName: 'Entidad',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'DescripcionCR',
                displayName: 'Identificador CR',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'Criticidad',
                displayName: 'Prioridad',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'EstadoProyecto',
                displayName: 'Estado del Proyecto',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'Horizonte',
                displayName: 'Horizonte',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'SectorNombre',
                displayName: 'Sector',
                enableHiding: false,
                width: '*'
            },
            {
                field: 'accion',
                displayName: 'Acción',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: vm.plantillaAccionesTabla,
                width: '*'
            }
        ];

        $timeout(function () {            
            
            // teste para grid simples
            //  vm.datos = [{
            //     Criticidad: "Baja",
            //     DescripcionCR: "Territorial",
            //     EstadoProyecto: "Viable",
            //     FechaCreacion: "2018-07-02T23:12:07.817",
            //     Horizonte: "2016 - 2016",
            //     IdObjetoNegocio: "2017004150017",
            //     NombreEntidad: "Boyacá",
            //     NombreObjetoNegocio: "Fortalecimiento organizacional de las asociaciones de productores agropecuarios del departamento de  Boyacá",
            //     ProyectoId: 12351,
            //     SectorNombre: "Agricultura y desarrollo rural"
            // }];

            

            
            // teste para grid hierarquica
            vm.datosSub = [{
                entidad: "Boyacá",
                sector: "Trámite",
                tipoEntidad: "Territorial",
                subGridOptions: {
                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                    paginationPageSize: 5,
                    expandableRowHeight: 250,
                    data: [{
                        Criticidad: "Baja",
                        DescripcionCR: "Territorial",
                        EstadoProyecto: "Viable",
                        FechaCreacion: "2018-07-02T23:12:07.817",
                        Horizonte: "2016 - 2016",
                        IdObjetoNegocio: "2017004150017",
                        NombreEntidad: "Boyacá",
                        NombreObjetoNegocio: "Fortalecimiento organizacional de las asociaciones de productores agropecuarios del departamento de  Boyacá",
                        ProyectoId: 12351,
                        SectorNombre: "Agricultura y desarrollo rural"
                    },
                    {
                        Criticidad: "Baja",
                        DescripcionCR: "Territorial",
                        EstadoProyecto: "Viable",
                        FechaCreacion: "2018-07-02T23:12:07.817",
                        Horizonte: "2016 - 2016",
                        IdObjetoNegocio: "2017004150017",
                        NombreEntidad: "Boyacá",
                        NombreObjetoNegocio: "Fortalecimiento organizacional de las asociaciones de productores agropecuarios del departamento de  Boyacá",
                        ProyectoId: 12351,
                        SectorNombre: "Agricultura y desarrollo rural"
                    }]
                }                            
            },
            {
                entidad: "Boyacá 2",
                sector: "Trámite 2",
                tipoEntidad: "Territorial",
                subGridOptions: {
                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                    paginationPageSize: 5,
                    expandableRowHeight: 250,
                    data: [{
                        Criticidad: "Baja",
                        DescripcionCR: "Territorial",
                        EstadoProyecto: "Viable",
                        FechaCreacion: "2018-07-02T23:12:07.817",
                        Horizonte: "2016 - 2016",
                        IdObjetoNegocio: "2017004150017",
                        NombreEntidad: "Boyacá",
                        NombreObjetoNegocio: "Fortalecimiento organizacional de las asociaciones de productores agropecuarios del departamento de  Boyacá",
                        ProyectoId: 12351,
                        SectorNombre: "Agricultura y desarrollo rural"
                    },
                    {
                        Criticidad: "Baja",
                        DescripcionCR: "Territorial",
                        EstadoProyecto: "Viable",
                        FechaCreacion: "2018-07-02T23:12:07.817",
                        Horizonte: "2016 - 2016",
                        IdObjetoNegocio: "2017004150017",
                        NombreEntidad: "Boyacá",
                        NombreObjetoNegocio: "Fortalecimiento organizacional de las asociaciones de productores agropecuarios del departamento de  Boyacá",
                        ProyectoId: 12351,
                        SectorNombre: "Agricultura y desarrollo rural"
                    }]
                }                            
            }];
    
            vm.datos = [{
                entidad: "Boyacá",
                sector: "Agricultura y desarrollo rural",
                tipoEntidad: "Territorial",
                subGridOptions: {
                    data: vm.datosSub                    
                }                            
            }];
        
            
        }, 2000);


        /* configuração para grid hierarquica. Informar o número de níveis superiores (antes dos dados da grid). 
        No caso abaixo, segue a configuração da grid de trámites. No caso da tela de proyectos a configuração seria  vm.opciones.nivelJerarquico: 1 
        Pode-se aplicar quantos níveis forem necessários.        
        */
        // vm.opciones = {
        //     nivelJerarquico: 2
        // }

        /* configuração para grid simples.
        basta não informar um input de opciones ou se ter não conter a propriedade nivelJerarquico */
        //vm.opciones = {};

        /* adicionar ou substituir configurações na grid pré-determinadas na grid principal. 
        Isto é definido pela propriedade gridOptions, onde nela, são informadas as configurações customizadas.

        Neste exemplo, está sendo modificado para mostrar o header na grid principal e adicionando a configuração de paginações exibidas.

        A propriedade subGridOptions no atributo Data ela é como se fosse um gridOptions para as grids internas. 
        Desta forma, é possível passar configurações para a mesma que irão se refletir no componente. 
        */
        vm.opciones = {
            nivelJerarquico: 2,
            gridOptions: {
                showHeader: true,
                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                paginationPageSize: 5,
                expandableRowHeight: 200
            }
        };



        vm.accionTeste1 = function (row) {
            alert('accionTeste1:' + JSON.stringify(row));
        }

        vm.accionTeste2 = function (row) {
            alert('accionTeste2:' + JSON.stringify(row));
        }

        vm.accionTeste3 = function (row) {
            alert('accionTeste3:' + JSON.stringify(row));
        }
    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').controller('controladorTabla', controladorTabla);
})();