"use client"
import React from 'react';
import ProductTable from './components/ProductTable';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';


const MyPage: React.FC = () => {
  return (
    <div>
      <AppBar position="static">
        <Toolbar sx={{ justifyContent: 'center' }}>
          <Typography variant="h6">Product Inventory</Typography>
        </Toolbar>
      </AppBar>
      <br/>
      <ProductTable/>
    </div>
  );
};

export default MyPage;