import React, { useState, useEffect } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
  Paper,
  TableSortLabel,
  Toolbar,
  Snackbar,
  Alert
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import ProductModal from './ProductModal/ProductModal';
import DeleteModal from './DeleteModal/DeleteModal';
import { getProducts, deleteProduct } from '../api/api';
import { Product } from '../interfaces/Product';
import SearchBar from './SearchBar/SearchBar';

type ProductKeys = keyof Product;

export default function ProductTable() {
  const [products, setProducts] = useState<Product[]>([]);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);
  const [filteredProducts, setFilteredProducts] = useState<Product[]>([]);
  const [isEditOpen, setIsEditOpen] = useState(false);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isCreateOpen, setIsCreateOpen] = useState(false); 
  const [orderBy, setOrderBy] = useState<ProductKeys>('id');
  const [order, setOrder] = useState<'asc' | 'desc'>('asc');
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [snackbarSeverity, setSnackbarSeverity] = useState<'success' | 'error'>('success');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const fetchedProducts = await getProducts();
      setProducts(fetchedProducts);
      setFilteredProducts(fetchedProducts);
    } catch (error) {
      console.error('Error fetching products:', error);
    }
  };

  const handleSort = (property: ProductKeys) => {
    const isAsc = orderBy === property && order === 'asc';
    setOrderBy(property);
    setOrder(isAsc ? 'desc' : 'asc');
  };

  const handleSearch = (query: string) => {
    const filteredProducts = products.filter((product) =>
    product.name.toLowerCase().includes(query.toLowerCase()) ||
    product.brand.toLowerCase().includes(query.toLowerCase())
  );

    setFilteredProducts(filteredProducts);
  };

  const sortedProducts = [...filteredProducts].sort((a, b) => {
    const aValue = a[orderBy];
    const bValue = b[orderBy];
    if (order === 'asc') {
      return typeof aValue === 'string' ? aValue.localeCompare(bValue as string) : aValue! - (bValue as number);
    } else {
      return typeof bValue === 'string' ? bValue.localeCompare(aValue as string) : bValue! - (aValue as number);
    }
  });

  const handleOpenEditModal = (product: Product) => {
    setSelectedProduct(product);
    setIsEditOpen(true);
  };

  const handleCloseEditModal = () => {
    setSelectedProduct(null);
    setIsEditOpen(false);
  };

  const handleEditSubmit = () => {
    setIsEditOpen(false);
    fetchData();
    setSnackbarSeverity('success');
    setSnackbarMessage('Product updated successfully!');
    setSnackbarOpen(true);
  };

  const handleOpenDeleteModal = (product: Product) => {
    setSelectedProduct(product);
    setIsDeleteOpen(true);
  };

  const handleCloseDeleteModal = () => {
    setSelectedProduct(null);
    setIsDeleteOpen(false);
  };

  const handleDeleteSubmit = async () => {
    try {
      setIsDeleteOpen(false);
      fetchData();
      setSnackbarSeverity('success');
      setSnackbarMessage('Product deleted successfully!');
      setSnackbarOpen(true);
    } catch (error) {
      console.error('Error deleting product:', error);
      setSnackbarSeverity('error');
      setSnackbarMessage('Error deleting product');
      setSnackbarOpen(true);
    }
  };

  const handleOpenCreateModal = () => {
    setIsCreateOpen(true);
  };

  const handleCloseCreateModal = () => { 
    setIsCreateOpen(false);
  };

  const handleCreateSubmit = () => {
    setIsCreateOpen(false);
    fetchData();
    setSnackbarSeverity('success');
    setSnackbarMessage('Product created successfully!');
    setSnackbarOpen(true);
  };

  const handleSnackbarClose = () => {
    setSnackbarOpen(false);
  };

  return (
    <div>
        <Toolbar sx={{ justifyContent: 'center' }}>
              <SearchBar onSearch={handleSearch} />
        </Toolbar>
      <br/>
      <div style={{ textAlign: 'right', padding: '10px', width: '91%'}}>
        <Button variant="contained" onClick={handleOpenCreateModal}>Add Product</Button>
      </div>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>
                <TableSortLabel
                  active={orderBy === 'id'}
                  direction={orderBy === 'id' ? order : 'asc'}
                  onClick={() => handleSort('id')}
                >
                  Id
                </TableSortLabel>
              </TableCell>
              <TableCell>
                <TableSortLabel
                  active={orderBy === 'name'}
                  direction={orderBy === 'name' ? order : 'asc'}
                  onClick={() => handleSort('name')}
                >
                  Name
                </TableSortLabel>
              </TableCell>
              <TableCell>
                <TableSortLabel
                  active={orderBy === 'brand'}
                  direction={orderBy === 'brand' ? order : 'asc'}
                  onClick={() => handleSort('brand')}
                >
                  Brand
                </TableSortLabel>
              </TableCell>
              <TableCell>
                <TableSortLabel
                  active={orderBy === 'price'}
                  direction={orderBy === 'price' ? order : 'asc'}
                  onClick={() => handleSort('price')}
                >
                  Price
                </TableSortLabel>
              </TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {sortedProducts.map((product) => (
              <TableRow key={product.id}>
                <TableCell>{product.id}</TableCell>
                <TableCell>{product.name}</TableCell>
                <TableCell>{product.brand}</TableCell>
                <TableCell>{product.price} EUR</TableCell>
               <TableCell>
                  <Button 
                    variant="contained"
                    onClick={() => handleOpenEditModal(product)}
                    color="info"
                    endIcon={<EditIcon />}
                    size="small"
                    style={{ marginRight: '8px' }}
                  >
                    Edit
                  </Button>
                  <Button
                    variant="contained"
                    onClick={() => handleOpenDeleteModal(product)}
                    color="error"
                    endIcon={<DeleteIcon />}
                    size="small"
                    style={{  marginRight: '8px' }}
                  >
                    Delete
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <Snackbar open={snackbarOpen} autoHideDuration={6000} onClose={handleSnackbarClose}   anchorOrigin={{vertical: 'bottom', horizontal: 'right'}}>
        <Alert onClose={handleSnackbarClose} severity={snackbarSeverity}>
          {snackbarMessage}
        </Alert>
      </Snackbar>
      {isEditOpen && (
        <ProductModal
          product={selectedProduct!}
          isEditOpen={isEditOpen}
          onClose={handleCloseEditModal}
          onSubmit={handleEditSubmit}
        />
      )}
      {isDeleteOpen && (
        <DeleteModal
          product={selectedProduct!}
          isDeleteOpen={isDeleteOpen}
          onClose={handleCloseDeleteModal}
          onSubmit={handleDeleteSubmit}
        />
      )}
      {isCreateOpen && ( 
        <ProductModal
          product={{ name: '', brand: '', price: 0 }} 
          isEditOpen={isCreateOpen}
          isCreate={true} 
          onClose={handleCloseCreateModal}
          onSubmit={handleCreateSubmit} 
        />
      )}
      
    </div>
  );
}
