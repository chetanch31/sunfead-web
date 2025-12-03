import Navbar from "@/components/Navbar";
import Footer from "@/components/Footer";
import ProductCard from "@/components/ProductCard";

const allProducts = [
  {
    id: "2",
    name: "Dalia",
    image: "/static/products/dalia_10_150gm.jpg",
    category: "Flour & Grains",
    description: "Nutritious and wholesome dalia, ideal for a healthy breakfast.",
    variants: [
      { id: "2-1", weight: "150gm", mrp: 10, price: 10 },
      { id: "2-2", weight: "500gm", mrp: 45, price: 45 },
    ],
  },
  {
    id: "3",
    name: "Fusilli Pasta",
    image: "/static/products/Fusilli_Pasta_10_80gms.jpg",
    category: "Pasta",
    description: "Premium fusilli pasta with perfect spiral shape for holding sauces.",
    variants: [
      { id: "3-1", weight: "80gm", mrp: 10, price: 10 },
      { id: "3-3", weight: "120gm", mrp: 500, price: 500 },
      { id: "3-4", weight: "500gm", mrp: 120, price: 120 }, // updated
    ],
  },
  {
    id: "4",
    name: "Macaroni",
    image: "/static/products/Macroni_10_80gms_page-0001.jpg",
    category: "Pasta",
    description: "Delicious macaroni, perfect for quick and tasty meals.",
    variants: [
      { id: "4-1", weight: "80gm", mrp: 10, price: 10 },
      { id: "4-3", weight: "500gm", mrp: 120, price: 120 }, // updated
    ],
  },
  {
    id: "5",
    name: "Premium Macaroni",
    image: "/static/products/premiummacaroni_590_5kg.jpg",
    category: "Pasta",
    description:
      "Premium quality macaroni in bulk pack for families and food businesses.",
    variants: [{ id: "5-1", weight: "5kg", mrp: 590, price: 590 }],
  },
  {
    id: "6",
    name: "Penne Pasta",
    image: "/static/products/pennepasta_10_80gm.jpg",
    category: "Pasta",
    description: "Classic penne pasta, ideal for baked dishes and pasta salads.",
    variants: [
      { id: "6-1", weight: "80gm", mrp: 10, price: 10 },
      { id: "6-3", weight: "450gm", mrp: 70, price: 70 }, // updated
    ],
  },
  {
    id: "7",
    name: "Shell Pasta",
    image: "/static/products/Shell_Pasta_10_80gms_page-0001.jpg",
    category: "Pasta",
    description: "Shell-shaped pasta perfect for creamy sauces and casseroles.",
    variants: [
      { id: "7-1", weight: "80gm", mrp: 10, price: 10 },
      { id: "7-2", weight: "450gm", mrp: 70, price: 70 },
    ],
  },
  {
    id: "8",
    name: "Plain Vermicelli",
    image: "/static/products/plainvermicelli_10_80gm.jpg",
    category: "Vermicelli",
    description:
      "Fine plain vermicelli, ideal for sweet and savory dishes.",
    variants: [
      { id: "8-1", weight: "80gm", mrp: 10, price: 10 },
      { id: "8-2", weight: "450gm", mrp: 65, price: 65 }, // updated
    ],
  },
  {
    id: "9",
    name: "Roasted Vermicelli",
    image: "/static/products/roastedvermicelli_10_75gm.jpg",
    category: "Vermicelli",
    description: "Pre-roasted vermicelli for quick and convenient cooking.",
    variants: [
      { id: "9-1", weight: "75gm", mrp: 10, price: 10 },
      { id: "9-2", weight: "450gm", mrp: 70, price: 70 },
    ],
  },
  {
    id: "10",
    name: "Poha",
    image: "/static/products/poha_10_100gm.jpg",
    category: "Flour & Grains",
    description: "Light and nutritious poha, perfect for a quick breakfast.",
    variants: [
      { id: "10-1", weight: "100gm", mrp: 10, price: 10 },
      { id: "10-2", weight: "500gm", mrp: 70, price: 70 },
    ],
  },
  {
    id: "11",
    name: "Soya Chunks",
    image: "/static/products/soyachunks_10_50gm.jpg",
    category: "Pulses & Protein",
    description:
      "Protein-rich soya chunks, great for healthy and nutritious meals.",
    variants: [{ id: "11-1", weight: "50gm", mrp: 10, price: 10 }],
  },

  // moved to last
  {
    id: "1",
    name: "Besan",
    image: "/static/products/Besan_3590_35kg.jpg",
    category: "Flour & Grains",
    description:
      "High-quality besan, perfect for making pakoras, laddoos, and other traditional dishes.",
    variants: [{ id: "1-1", weight: "35kg", mrp: 3590, price: 3590 }],
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
