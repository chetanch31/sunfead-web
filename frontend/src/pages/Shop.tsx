import Navbar from "@/components/Navbar";
import Footer from "@/components/Footer";
import ProductCard from "@/components/ProductCard";

const allProducts = [
  {
    id: "1",
    name: "Sunfead Besan",
    image: "/besan.jpg",
    category: "Besan",
    description: "High-quality besan, perfect for making pakoras, laddoos, and other traditional dishes.",
    variants: [
      { id: "1-10", weight: "10g", mrp: 10, price: 10 },
    ],
  },
  {
    id: "2",
    name: "Sunfead Daliya",
    image: "/daliya.jpg",
    category: "Daliya",
    description: "Nutritious and wholesome daliya, ideal for a healthy breakfast.",
    variants: [
      { id: "2-10", weight: "10g", mrp: 10, price: 10 },
    ],
  },
  {
    id: "3",
    name: "Sunfead Macaroni",
    image: "/macaroni.jpg",
    category: "Macaroni",
    description: "Delicious macaroni, perfect for quick and tasty meals.",
    variants: [
      { id: "3-10", weight: "10g", mrp: 10, price: 10 },
    ],
  },
  {
    id: "4",
    name: "Sunfead Pasta",
    image: "/pasta.jpg",
    category: "Pasta",
    description: "Premium pasta made from the finest wheat.",
    variants: [
      { id: "4-10", weight: "10g", mrp: 10, price: 10 },
    ],
  },
  {
    id: "5",
    name: "Sunfead Poha",
    image: "/poha.jpg",
    category: "Poha",
    description: "Light and nutritious poha, perfect for a quick breakfast.",
    variants: [
      { id: "5-10", weight: "10g", mrp: 10, price: 10 },
    ],
  },
  {
    id: "6",
    name: "Sunfead Soya Chunks",
    image: "/soya_chunks.jpg",
    category: "Soya Chunks",
    description: "Protein-rich soya chunks, great for healthy meals.",
    variants: [
      { id: "6-10", weight: "10g", mrp: 10, price: 10 },
    ],
  },
  {
    id: "7",
    name: "Sunfead Vermicelli",
    image: "/vermicelli.jpg",
    category: "Vermicelli",
    description: "Fine vermicelli, ideal for sweet and savory dishes.",
    variants: [
      { id: "7-10", weight: "10g", mrp: 10, price: 10 },
    ],
  },
];

const Shop = () => {
  return (
    <div className="min-h-screen bg-background">
      <Navbar />
      
      {/* Header */}
      <section className="py-12 md:py-16 bg-card border-b">
        <div className="container mx-auto px-4">
          <div className="max-w-3xl mx-auto text-center">
            <h1 className="text-4xl md:text-5xl font-display font-bold mb-4">
              Our <span className="text-gradient">Products</span>
            </h1>
            <p className="text-lg text-muted-foreground">
              Explore our complete range of authentic Indian snacks and staples, 
              made with love and traditional recipes
            </p>
          </div>
        </div>
      </section>

      {/* Products Grid */}
      <section className="py-16 md:py-24">
        <div className="container mx-auto px-4">
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {allProducts.map((product) => (
              <ProductCard key={product.id} {...product} />
            ))}
          </div>
        </div>
      </section>

      <Footer />
    </div>
  );
};

export default Shop;
