function changePage(e, idParam, countVisiblePagesParam, countPagesParam, url, idBox) {
    var self = e;
    var id = "#" + idParam;
    var page = 1;
    var currPage = parseInt($(id).find(".active").children().text());
    var currFirstPage = parseInt($(id + " > ul > li:eq(1)").children().text());
    var currLastPage = parseInt($(id + " > ul > li:last-child").prev().children().text());
    if ($(self).attr("aria-label") == "Previous") {
        if (currPage > currFirstPage) {
            self = $(id).find(".active").prev().children();
        }

    } else if ($(self).attr("aria-label") == "Next") {
        if (currPage < currLastPage) {
            self = $(id).find(".active").next().children();
        }
    }
    page = parseInt($(self).text());

    var countVisiblePages = parseInt(countVisiblePagesParam);
    var countPages = parseInt(countPagesParam);

    $(idBox).load(url, { "page": page }, function (response, status) {
        if (status == "success") {
            $(id).find(".active").removeClass("active");

            //========= prev / next enabling
            if (page == 1) {
                $(id + " > ul > li:first-child").addClass("disabled");
            } else {
                $(id + "> ul > li:first-child").removeClass("disabled");
            }
            if (page == countPages) {
                $(id + " > ul > li:last-child").addClass("disabled");
            } else {
                $(id + " > ul > li:last-child").removeClass("disabled");
            }
            var casePagination = 0;
            if (countPages > countVisiblePages) {
                // go ahead
                if (page >= (currFirstPage + Math.ceil(countVisiblePages / 2)) && page <= (countPages - Math.ceil(countVisiblePages / 2) + 1)) {
                    casePagination = 1;
                } else {
                    // go back
                    if (page < (currFirstPage + Math.ceil(countVisiblePages / 2)) && currFirstPage > 1) {
                        casePagination = 2;
                    }
                }
            }
            if (casePagination == 1 || casePagination == 2) {
                var newFirstPage;
                if (casePagination == 1) {
                    newFirstPage = page - Math.ceil(countVisiblePages / 2) + 1;
                } else {
                    newFirstPage = page - Math.ceil(countVisiblePages / 2) + 1;
                }

                if (newFirstPage < 1) newFirstPage = 1;
                if (newFirstPage > countPages - Math.ceil(countVisiblePages / 2)) newFirstPage = countPages - countVisiblePages;

                $(id + " > ul > li").each(function (index) {
                    if (index != 0 && index < countVisiblePages + 1) {
                        $(this).children().text(newFirstPage);
                        if (newFirstPage == page) {
                            $(this).addClass("active");
                        }
                        newFirstPage = newFirstPage + 1;
                    }
                });
            }
            else {
                $(self).parent().addClass("active");
            }
        }
    });
}
