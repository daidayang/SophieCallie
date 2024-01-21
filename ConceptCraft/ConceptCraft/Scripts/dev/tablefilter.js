var Table_Filter = function( table, header, body)
{
    this.THeader = $(header);
    this.Table = $(table);
    this.TBody = $(body);
    this.ColumFilters = [];
    this.ColumUniqueFilters = [];
    this.ColumSortings = [];
    this.FilterCounter = 0;

    this.add_filter_icons = function () {
        var table_filter = this;
        this.THeader.find('th').each(function (idx) {
            var th = $(this);
            if (th.hasClass('footable-sortable')) {
                th.append($("<span><a class=\"table_filter_link\" data-index=\"" + idx + "\"><i class=\"fa fa-filter\"></i></a></span>"));
            }
        });
        table_filter.TBody.find('tr').each(function (idx) {
            $(this).attr('data-index', idx);
        });
        this.THeader.find('.table_filter_link').unbind('click').bind('click', function () {
            var offset = $(this).offset();
            $('#modal_tablefilter').modal('show').css({ 'left': offset.left -150, 'top':offset.top -10, 'width': 300, 'height': 550 });
            $('.modal-backdrop').css({ 'opacity': 0 }).click(function () {
                if ($('#modal_tablefilter').is(':visible')) {
                    $('#modal_tablefilter').modal('hide');
                }
            });            
               
            
            $('#modal_filters').empty();
            $('#modal_tablefilter').find('.select_all').unbind('click').bind('click', function () {
                $('#modal_filters').find('input').prop("checked", true);
            });

            $('#modal_tablefilter').find('.unselect_all').unbind('click').bind('click', function () {
                $('#modal_filters').find('input').prop("checked", false);
            });

            $('#modal_tablefilter').find('.modal-body').slimScroll(
               { height: 300, alwaysVisible: false, color: '#888', allowPageScroll: false, disableIfFit: true }
            );


            var index = $(this).parents('th:first').attr('data-index');

            $('#modal_tablefilter').find('.btn-apply').attr('data-index', index);

            //Init filters
            for (var i in table_filter.ColumUniqueFilters)
            {
                if (table_filter.ColumUniqueFilters[i].column == index) {
                    var filters = table_filter.ColumUniqueFilters[i].filters;
                    for (var j in filters) {
                        var text = filters[j].text;
                        if (filters[j].show) {
                            $('#modal_filters').append($("<div><label style='width:100%;'>" + text + "   <input type='checkbox' class='pull-right' checked value='" + text + "'></label></div>"));
                        }
                        else
                        {
                            $('#modal_filters').append($("<div><label style='width:100%;'>" + text + "   <input type='checkbox' class='pull-right' value='" + text + "'></label></div>"));
                        }
                    }
                }
            }

            //Init Sorting
            for (var i in table_filter.ColumSortings) {
                if (table_filter.ColumSortings[i].column == index) {
                    if (table_filter.ColumSortings[i].sort == 0)
                    {
                        $('#rdo_filter_table_sort_a').prop('checked',true);
                    }
                    else
                    {
                        $('#rdo_filter_table_sort_d').prop('checked', true);
                    }
                }
            }
        });
    }

    this.add_filter_optoins = function () {
        var table_filter = this;
        this.THeader.find('th').each(function (idx) {
            var th = $(this);          
            if( th.hasClass('footable-sortable') == true )
            {
                th.attr('data-index', idx);
                var tdValues = [];
                var tdUniqueValues = [];
                var tdUniqueValuesFilter = [];
                table_filter.TBody.find('tr').each(function (rowIdx) {
                    var tr = $(this);
                    var td = tr.find('td')[idx];
                    td = $(td);
                    var text = td.text();
                    tdValues.push({ 'text': text, 'show': true, 'row': rowIdx });

                    var found = false;
                    for( var i in tdUniqueValues)
                    {
                        if (tdUniqueValues[i] == text)
                        {
                            found = true;
                            break;
                        }
                    }

                    if( found == false)
                    {
                        tdUniqueValues.push(text);
                        
                    }
                });

                tdUniqueValues.sort();
                for (var i in tdUniqueValues) {
                    tdUniqueValuesFilter.push({ 'text': tdUniqueValues[i], 'show': true });
                }

                table_filter.ColumFilters.push({ "column": idx, "filters": tdValues });
                table_filter.ColumUniqueFilters.push({ "column": idx, "filters": tdUniqueValuesFilter });
                table_filter.ColumSortings.push({ 'column': idx, 'sort': 0 });
            }
        });
    }

    this.apply_clicked = function()
    {
        var table_filter = this;
       
        $('#modal_tablefilter').find('.btn-apply').unbind('click').bind('click', function () {
            var index = $(this).attr('data-index');

            for( var i in table_filter.ColumSortings)
            {
                var sort = table_filter.ColumSortings[i];
                if( sort.column == index)
                {
                    if( $('#rdo_filter_table_sort_a').is(":checked") )
                    {
                        sort.sort = 0;
                    }
                    else if( $('#rdo_filter_table_sort_d').is(":checked") )
                    {
                        sort.sort = 1;
                    }
                }
            }

            var hasUnchecked = false;

            var checkedFilter = [];
            $('#modal_filters').find('input').each(function () {
                if( $(this).is(":checked"))
                {
                    checkedFilter.push($(this).val());
                }
                else
                {
                    hasUnchecked = true;
                }
            });

            if (hasUnchecked == true)
            {
                $('.table_filter_link').each(function () {
                    if( $(this).attr('data-index') == index)
                    {
                        $(this).addClass('table_filter_link_active');
                        $(this).parents('th:first').addClass('table_filter_link_active');
                    }
                });
            }
            else
            {
                $('.table_filter_link').each(function () {
                    if ($(this).attr('data-index') == index) {
                        $(this).removeClass('table_filter_link_active');
                        $(this).parents('th:first').removeClass('table_filter_link_active');
                    }
                });

            }
           

            for (var i in table_filter.ColumUniqueFilters) {
                if (table_filter.ColumUniqueFilters[i].column == index) {
                    var filters = table_filter.ColumUniqueFilters[i].filters;
                    for (var j in filters) {
                        var text = filters[j].text;
                        var found = false;
                        for (var m in checkedFilter) {
                            if (checkedFilter[m] == text) {
                                found = true;
                                break;
                            }
                        }

                        if(found == true )
                        {
                            filters[j].show = true;
                        }
                        else
                        {
                            filters[j].show = false;
                        }
                    }
                }
            }

            for (var i in table_filter.ColumFilters) {
                if (table_filter.ColumFilters[i].column == index) {
                    var filters = table_filter.ColumFilters[i].filters;
                    for (var j in filters) {
                        var text = filters[j].text;
                        var found = false;
                        for (var m in checkedFilter) {
                            if (checkedFilter[m] == text) {
                                found = true;
                                break;
                            }
                        }

                        if (found == true) {
                            filters[j].show = true;
                        }
                        else {
                            filters[j].show = false;
                        }
                    }
                }
            }

            var rows = table_filter.TBody.find('tr');
            rows.sort(function (a, b) {         
                var A = $(a).children('td').eq(index).text().toUpperCase();       
                var B = $(b).children('td').eq(index).text().toUpperCase();
                if ($('#rdo_filter_table_sort_a').is(":checked")) {
                    if (isNaN(A) == false  && isNaN(B) == false) {
                        return A - B;
                    }
                    else
                    {
                        return A.localeCompare(B);
                    }
                }
                else if ($('#rdo_filter_table_sort_d').is(":checked")) {
                    
                    if (isNaN(A) == false && isNaN(B) == false) {
                        return B - A;
                    }
                    else {
                        return B.localeCompare(A);
                    }
                }
            });

            $.each(rows, function (idx, row) {
                var show = true;

                $.each($(row).find('td'), function (tdIdx, td) {
                    var tdText = $(td).text();

                    for (var i in table_filter.ColumUniqueFilters) {

                        if (table_filter.ColumUniqueFilters[i].column == tdIdx) {
                            var filters = table_filter.ColumUniqueFilters[i].filters;
                            for (var j in filters) {
                                var text = filters[j].text;
                                if( text ==  tdText && filters[j].show == false)
                                {
                                    show = false;
                                    return false;
                                }
                            }
                        }                        
                    }
                });

                if (show == false)
                {
                    $(row).hide();
                }
                else
                {
                    $(row).show();
                }

                table_filter.TBody.append(row);          
            });


            $('#modal_tablefilter').modal('hide');
        });
    }

    this.add_clear_filter_button = function () {
        var table_filter = this;
        table_filter.THeader.find('tr:first').find('th:last').css('position', 'relative');
        table_filter.THeader.find('tr:first').find('th:last').append($("<button class='btn btn-green btn-sm btn_clear_filters' id='btn_clear_table_filters'>Clear Filters</button>"));
    }

    this.clear_filter_clicked = function()
    {
        var table_filter = this;
        $('#btn_clear_table_filters').unbind('click').bind('click', function () {
            var rows = table_filter.TBody.find('tr');
            rows.sort(function (a, b) {
                var A = parseInt( $(a).attr('data-index'));
                var B = parseInt($(b).attr('data-index'));
               
                return A - B;
                
            });

            $.each(rows, function (idx, row) {      
                $(row).show();
                table_filter.TBody.append(row);
            });

            $('.table_filter_link').each(function () {
               
                $(this).removeClass('table_filter_link_active');
                $(this).parents('th:first').removeClass('table_filter_link_active');
            });

            for (var idx in table_filter.ColumFilters) {
                var filters = table_filter.ColumFilters[idx].filters;
                for (var m in filters) {
                    filters[m].show = true;
                }
            };

            for( var idx in table_filter.ColumUniqueFilters ){
                var filters = table_filter.ColumUniqueFilters[idx].filters;
                for (var m in filters) {
                    filters[m].show = true;
                }
            };

            for( var idx in table_filter.ColumSortings) {
                table_filter.ColumSortings[idx].sort = 0;
            };
        });
    }

    this.init = function()
    {
        this.add_clear_filter_button();
        this.add_filter_icons();
        this.add_filter_optoins();
        this.apply_clicked();
        this.clear_filter_clicked();
    }
}