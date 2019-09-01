import React from 'react';
import shortid from 'shortid';
import Skeleton from '@material-ui/lab/Skeleton';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';
import Avatar from '@material-ui/core/Avatar';
import PersonIcon from '@material-ui/icons/Person';

function TableBodySkeleton() {
    return (
        <TableBody>
            {[...Array(10)].map(() => (
                <TableRow key={shortid.generate()}>
                    <TableCell><Avatar><PersonIcon/></Avatar></TableCell>
                    {[...Array(13)].map(() => (
                        <TableCell key={shortid.generate()}><Skeleton variant="text"/></TableCell>
                    ))}
                </TableRow>
            ))}

        </TableBody>
    );
}

export default TableBodySkeleton;
