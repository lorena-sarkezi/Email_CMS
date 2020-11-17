import React from 'react';

import { Pagination } from 'antd';

const TablePagination = props => {

    const handleChangePageNumber = value => {
        let data = {...props.pageState};
        data.current = value;

        props.setPageState(data);
    }

    return(
        <Pagination total={props.total} onChange={(page,pageSize) => handleChangePageNumber(page)} />
    )
}

export default TablePagination;