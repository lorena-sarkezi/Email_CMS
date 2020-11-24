import React from 'react';

import { Pagination } from 'antd';

const TablePagination = ({pageState, setPageState,total}) => {

    const handleChangePageNumber = value => {
        let data = {...pageState};
        data.current = value;

        setPageState(data);
    }

    const handleChangePageSize = (value) => {
        let data = {...pageState};
        data.pageSize = value;

        setPageState(data);
    }

    return(
        <Pagination 
            showSizeChanger
            total={total}
            onChange={(page,pageSize) => handleChangePageNumber(page)} 
            onShowSizeChange={(current,pageSize) => handleChangePageSize(pageSize)}/>
    )
}

export default TablePagination;