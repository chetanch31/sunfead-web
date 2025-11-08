import ProductCard from "./ProductCard";
import namkeenImg from "@/assets/product-namkeen.jpg";
import ladooImg from "@/assets/product-ladoo.jpg";
import barfiImg from "@/assets/product-barfi.jpg";
import chakliImg from "@/assets/product-chakli.jpg";

const products = [
  {
    id: "1",
    name: "Classic Namkeen Mix",
    image: namkeenImg,
    category: "Namkeen",
    variants: [
      { id: "1-250", weight: "250g", mrp: 150, price: 120 },
      { id: "1-500", weight: "500g", mrp: 280, price: 230 },
      { id: "1-1kg", weight: "1kg", mrp: 520, price: 440 },
    ],
    isNew: true,
    discount: 20,
  },
  {
    id: "2",
    name: "Besan Ladoo",
    image: ladooImg,
    category: "Sweets",
    variants: [
      { id: "2-250", weight: "250g", mrp: 200, price: 180 },
      { id: "2-500", weight: "500g", mrp: 380, price: 350 },
      { id: "2-1kg", weight: "1kg", mrp: 720, price: 680 },
    ],
    discount: 10,
  },
  {
    id: "3",
    name: "Kaju Katli",
    image: barfiImg,
    category: "Sweets",
    variants: [
      { id: "3-250", weight: "250g", mrp: 400, price: 380 },
      { id: "3-500", weight: "500g", mrp: 780, price: 750 },
    ],
  },
  {
    id: "4",
    name: "Chakli (Murukku)",
    image: chakliImg,
    category: "Namkeen",
    variants: [
      { id: "4-200", weight: "200g", mrp: 120, price: 100 },
      { id: "4-400", weight: "400g", mrp: 220, price: 190 },
    ],
    isNew: true,
    discount: 15,
  },
];

const FeaturedProducts = () => {
  return (
    <section className="py-16 md:py-24 bg-background">
      <div className="container mx-auto px-4">
        <div className="text-center mb-12">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-display font-bold mb-4">
            Bestsellers
          </h2>
          <p className="text-lg text-muted-foreground max-w-2xl mx-auto">
            Our most loved treats, crafted with authentic recipes passed down through generations
          </p>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {products.map((product) => (
            <ProductCard key={product.id} {...product} />
          ))}
        </div>

        <div className="text-center mt-12">
          <a
            href="/shop"
            className="inline-block text-primary font-semibold hover:underline"
          >
            View All Products →
          </a>
        </div>
      </div>
    </section>
  );
};

export default FeaturedProducts;
