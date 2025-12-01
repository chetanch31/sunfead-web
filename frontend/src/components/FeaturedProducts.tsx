import ProductCard from "./ProductCard";
import { Link } from "react-router-dom";

const products = [
	{
		id: "1",
		name: "Sunfead Besan",
		image: "/besan.jpg",
		category: "Besan",
		variants: [{ id: "1-10", weight: "10g", mrp: 10, price: 10 }],
	},
	{
		id: "2",
		name: "Sunfead Daliya",
		image: "/daliya.jpg",
		category: "Daliya",
		variants: [{ id: "2-10", weight: "10g", mrp: 10, price: 10 }],
	},
	{
		id: "3",
		name: "Sunfead Macaroni",
		image: "/macaroni.jpg",
		category: "Macaroni",
		variants: [{ id: "3-10", weight: "10g", mrp: 10, price: 10 }],
	},
	{
		id: "4",
		name: "Sunfead Pasta",
		image: "/pasta.jpg",
		category: "Pasta",
		variants: [{ id: "4-10", weight: "10g", mrp: 10, price: 10 }],
	}
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
						Our most loved treats, crafted with authentic recipes passed down
						through generations
					</p>
				</div>

				<div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
					{products.map((product) => (
						<ProductCard key={product.id} {...product} />
					))}
				</div>

				<div className="text-center mt-12">
					<Link
						to="/shop"
						className="inline-block text-primary font-semibold hover:underline"
					>
						View All Products →
					</Link>
				</div>
			</div>
		</section>
	);
};

export default FeaturedProducts;
