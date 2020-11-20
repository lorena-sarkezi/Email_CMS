import React from 'react';

import { Pagination } from 'antd';

const TablePagination = props => {

    const handleChangePageNumber = value => {
        let data = {...props.pageState};
        data.current = value;

        props.setPageState(data);
    }

    const handleChangePageSize = (value) => {
        let data = {...props.pageState};
        data.pageSize = value;

        props.setPageState(data);
    }

    return(
        <Pagination 
            showSizeChanger
            total={props.total}
            onChange={(page,pageSize) => handleChangePageNumber(page)} 
            onShowSizeChange={(current,pageSize) => handleChangePageSize(pageSize)}/>
    )
}

export default TablePagination;